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
		private long LastMilliseconds { get; set; } = -1;

		private Queue<Vector2<int>> LocalPlayerWalkingQueue { get; }

		public ExtendedOpenTkClient(ClientConfiguration config, 
			OpenTKRsGraphicsContext graphicsObject, 
			IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext> 
				imageProducerFactory, IBufferFactory bufferFactory) 
			: base(config, graphicsObject, imageProducerFactory, bufferFactory)
		{
			LocalPlayerWalkingQueue = new Queue<Vector2<int>>(10);
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

		protected override bool HandlePacket81()
		{
			if(packetOpcode == 81)
			{
				int x = inStream.getShort();
				int y = inStream.getShort();

				playersObserved[playersObservedCount++] = LOCAL_PLAYER_ID;

				localPlayer.setPos(x, y, false);

				localPlayer.visible = true;
				localPlayer.lastUpdateTick = tick;
				
				localPlayer.standAnimationId = 0x328;
				localPlayer.standTurnAnimationId = 0x337;
				localPlayer.walkAnimationId = 0x333;
				localPlayer.turnAboutAnimationId = 0x334;
				localPlayer.turnLeftAnimationId = 0x335;
				localPlayer.turnRightAnimationId = 0x336;
				localPlayer.runAnimationId = 0x338;

				localPlayer.appearanceOffset = 336413342762192;
				localPlayer.appearance = new int[12] {0, 0, 0, 0, 275, 0, 285, 295, 259, 291, 300, 266};


				/*	playerProperties.put(DataType.SHORT, 0x328); // stand
					playerProperties.put(DataType.SHORT, 0x337); // stand turn
					playerProperties.put(DataType.SHORT, 0x333); // walk
					playerProperties.put(DataType.SHORT, 0x334); // turn 180
					playerProperties.put(DataType.SHORT, 0x335); // turn 90 cw
					playerProperties.put(DataType.SHORT, 0x336); // turn 90 ccw
					playerProperties.put(DataType.SHORT, 0x338); // run*/

				playerPositionY = 51;
				playerPositionX = 54;

				//updatePlayers(packetSize, inStream);
				loadingMap = false;
				packetOpcode = -1;
				return true;
			}

			return false;
		}

		public override void processGameLoop()
		{
			base.processGameLoop();

			long currentMilliseconds = TimeService.CurrentTimeInMilliseconds();
			
			if (LastMilliseconds == -1)
				LastMilliseconds = currentMilliseconds;
			else if (currentMilliseconds - 600 >= LastMilliseconds)
			{
				//It's been 600ms, time for another tick.
				LastMilliseconds = currentMilliseconds;
				ProcessGameTick();
			}
		}

		private void ProcessGameTick()
		{
			if (!LocalPlayerWalkingQueue.Any())
				return;

			Console.WriteLine($"X: {this.baseX + localPlayer.waypointX[0]} Y: {this.baseY + localPlayer.waypointY[0]}");

			Vector2<int> walkPoint = LocalPlayerWalkingQueue.Peek();

			int yOffet = Math.Sign(walkPoint.Y - localPlayer.waypointY[0]);
			int xOffet = Math.Sign(walkPoint.X - localPlayer.waypointX[0]);

			if (yOffet == 0 && 0 == xOffet)
			{
				LocalPlayerWalkingQueue.Dequeue();
				ProcessGameTick();
			}
			else
				localPlayer.setPos(xOffet + localPlayer.waypointX[0], yOffet + localPlayer.waypointY[0], false);
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
			LocalPlayerWalkingQueue.Clear();
			stream.putShort(walkingQueueX[currentIndex]);
			stream.putShort(walkingQueueY[currentIndex]);
			LocalPlayerWalkingQueue.Enqueue(new Vector2<int>(walkingQueueX[currentIndex], walkingQueueY[currentIndex]));

			for(int counter = 1; counter < maxPathSize; counter++)
			{
				currentIndex--;
				stream.putShort(walkingQueueX[currentIndex]);
				stream.putShort(walkingQueueY[currentIndex]);
				LocalPlayerWalkingQueue.Enqueue(new Vector2<int>(walkingQueueX[currentIndex], walkingQueueY[currentIndex]));
			}

			//Indicates if the ctrl key is pressed for running.
			stream.putByteC(base.keyStatus[5] != 1 ? 0 : 1);
		}
	}
}
