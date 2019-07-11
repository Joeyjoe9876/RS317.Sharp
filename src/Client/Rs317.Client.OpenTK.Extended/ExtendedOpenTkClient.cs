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
	}
}
