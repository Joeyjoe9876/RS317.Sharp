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
	public sealed class GladMMOUnityClient : RsUnityClient
	{
		public GameManager GameManagerService { get; }

		public event EventHandler OnLoginButtonClickedEvent;

		//Injected at connection time.
		public static IPeerPayloadSendService<GameClientPacketPayload> SendService { get; set; }

		public GladMMOUnityClient(ClientConfiguration config, UnityRsGraphics graphicsObject, GameManager gameManagerService) 
			: base(config, graphicsObject, new DefaultRunnableStarterStrategy(), new DefaultRsSocketFactory(new DefaultRunnableStarterStrategy()))
		{
			GameManagerService = gameManagerService;
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

			pathPoints.Add(new Vector3(walkingQueueX[currentIndex] + baseX, 0, walkingQueueY[currentIndex] + baseY));

			for(int counter = 1; counter < maxPathSize; counter++)
			{
				currentIndex--;
				pathPoints.Add(new Vector3(walkingQueueX[currentIndex] + baseX, 0, walkingQueueY[currentIndex] + baseY));
			}

			SendService.SendMessage(new ClientSetClickToMovePathRequestPayload(new PathBasedMovementData(pathPoints.ToArray(), 100)));
		}
	}
}
