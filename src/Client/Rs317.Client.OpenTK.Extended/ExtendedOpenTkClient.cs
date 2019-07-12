using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GladMMO;
using Refit;
using Reinterpret.Net;
using Rs317.Sharp;

namespace Rs317.Extended
{
	public sealed class ExtendedOpenTkClient : OpenTKClient
	{
		public ExtendedOpenTkClient(ClientConfiguration config, 
			OpenTKRsGraphicsContext graphicsObject, 
			IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext> 
				imageProducerFactory, IBufferFactory bufferFactory) 
			: base(config, graphicsObject, imageProducerFactory, bufferFactory)
		{

		}

		protected override int ReadPacketHeader(int currentAvailableBytes)
		{
			//3 byte header
			socket.read(inStream.buffer, 3);

			//2 byte length
			short payloadSize = inStream.buffer.Reinterpret<short>(0);
			packetSize = payloadSize;

			//1 byte opcode, for the client we don't preserve it in the stream since it doesn't use similar deserialization.
			packetOpcode = inStream.buffer[2] & 0xFF;

			packetSize--;
			currentAvailableBytes -= 3;
			return currentAvailableBytes;
		}

		//TODO: This is just initial/demo code. We need a real and proper implementation.
		protected override void HandleClientAuthentication(string playerUsername, string playerPassword, bool recoveredConnection)
		{
			//Dev hack to enable HTTPS for now.
			ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.CheckCertificateRevocationList = false;

			//TODO: Implement service discovery.
			IAuthenticationService service = RestService.For<IAuthenticationService>("https://auth.vrguardians.net/");

			JWTModel model = service.TryAuthenticate(new AuthenticationRequestModel(playerUsername, playerPassword))
				.ConfigureAwait(false).GetAwaiter().GetResult();

			Console.WriteLine($"Authentication Result: {model.isTokenValid} OptionalError: {model.Error}");

			if (model.isTokenValid)
			{
				ConnectToGameServer();
				HandleLoginSuccessful(2, false);

				SendSessionClaimRequest(model);
			}
		}

		private void SendSessionClaimRequest(JWTModel model)
		{
			//Now we must hand write the session claim packet
			stream.putShort((short) model.AccessToken.Length + 2 + 1); //2 bytes for the length prefixed access token and 1 for the opcode.
			stream.put((byte) RsClientNetworkOperationCode.SessionClaimRequest);
			stream.putString(model.AccessToken);
		}

		protected override void HandlePacketRecieveAntiCheatCheck()
		{
			//This prevents the client from disconnecting.
		}
		protected override void SendWalkPacket(int mouseClickType, int maxPathSize, int x, int currentIndex, int y)
		{
			if(currentWalkingQueueSize >= 92)
			{
				stream.putOpcode(36);
				stream.putInt(0);
				currentWalkingQueueSize = 0;
			}

			if(clickType == 0)
			{
				//opcode + size of pathpoints (x, y) so times 4 and then + 1 for length prefix array of pathpoints
				//and finally 1 byte for the run mode.
				stream.putShort(1 + (maxPathSize) * sizeof(short) * 2  + 1 + 1);
				stream.putOpcode(164);
				stream.put(maxPathSize);
			}

			if(clickType == 1)
			{
				stream.putOpcode(248);
				stream.put(maxPathSize + maxPathSize + 3 + 14);
			}

			if(clickType == 2)
			{
				stream.putOpcode(98);
				stream.put(maxPathSize + maxPathSize + 3);
			}

			//Send the X and Y as shorts, unlike 317.
			stream.putShort(walkingQueueX[currentIndex]);
			stream.putShort(walkingQueueY[currentIndex]);

			for(int counter = 1; counter < maxPathSize; counter++)
			{
				currentIndex--;
				stream.putShort(walkingQueueX[currentIndex]);
				stream.putShort(walkingQueueY[currentIndex]);
			}

			//Indicates if the ctrl key is pressed for running.
			stream.putByteC(base.keyStatus[5] != 1 ? 0 : 1);
		}
	}
}
