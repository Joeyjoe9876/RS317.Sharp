using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GladMMO;
using GladNet;
using Refit;
using Reinterpret.Net;
using Rs317.GladMMMO;
using Rs317.Sharp;
using UnityEngine;

namespace Rs317.GladMMO
{
	public sealed class GladMMOOpenTkClient : OpenTKClient
	{
		public GameManager GameManagerService { get; }

		public event EventHandler OnLoginButtonClickedEvent;

		//Injected at connection time.
		public static IPeerPayloadSendService<GameClientPacketPayload> SendService { get; set; }

		public GladMMOOpenTkClient(ClientConfiguration config, 
			OpenTKRsGraphicsContext graphicsObject, 
			IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext> imageProducerFactory, 
			IBufferFactory bufferFactory,
			GameManager gameManager) 
			: base(config, graphicsObject, imageProducerFactory, bufferFactory)
		{
			GameManagerService = gameManager;
			this.LoggedIn.OnVariableValueChanged += GameManagerService.OnLoginStateChanged;
		}

		protected override int ReadPacketHeader(int currentAvailableBytes)
		{
			return 0;
		}

		protected override void HandlePacketRecieveAntiCheatCheck()
		{
			//This prevents the client from disconnecting.
		}

		public override void processGameLoop()
		{
			GameManagerService.Service();
			base.processGameLoop();
		}

		protected override void OnLoginButtonClicked()
		{
			loginMessage1 = "";
			loginMessage2 = "Connecting to server...";
			drawLoginScreen(true);

			OnLoginButtonClickedEvent?.Invoke(this, EventArgs.Empty);
		}

		protected override void SendIdlePing()
		{
			//Just stub it out.
		}

		protected override void SendWalkPacket(int clickType, int maxPathSize, int x, int currentIndex, int y)
		{
			//TODO: Don't use unit vectors.
			List<Vector3> pathPoints = new List<Vector3>(currentIndex);

			for(int counter = 0; counter < maxPathSize; counter++)
			{
				pathPoints.Add(new Vector3(walkingQueueX[counter] + baseX, 0, walkingQueueY[counter] + baseY));
			}

			SendService.SendMessage(new ClientSetClickToMovePathRequestPayload(new PathBasedMovementData(pathPoints.ToArray(), 100)));
		}
	}
}
