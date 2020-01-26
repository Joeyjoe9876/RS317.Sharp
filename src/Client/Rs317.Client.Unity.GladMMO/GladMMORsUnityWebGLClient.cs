using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GladMMO;
using Rs317.GladMMMO;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class GladMMORsUnityWebGLClient : RsUnityWebGLClient, IRSClientLoginButtonPressedEventSubscribable
	{
		private GameManager GameManagerService { get; }

		public event EventHandler OnRunescapeLoginButtonPressed;

		public GladMMORsUnityWebGLClient(ClientConfiguration config, 
			UnityRsGraphics graphicsObject, 
			[JetBrains.Annotations.NotNull] MonoBehaviour clientMonoBehaviour, 
			IRsSocketFactory socketFactory, 
			ITaskDelayFactory taskDelayFactory,
			[JetBrains.Annotations.NotNull] GameManager rootGameManager) 
			: base(config, graphicsObject, clientMonoBehaviour, socketFactory, taskDelayFactory)
		{
			GameManagerService = rootGameManager ?? throw new ArgumentNullException(nameof(rootGameManager));
			this.LoggedIn.OnVariableValueChanged += GameManagerService.OnLoginStateChanged;
		}

		protected override Task<int> ReadPacketHeader(int currentAvailableBytes)
		{
			return Task.FromResult<int>(0);
		}

		protected override async Task HandlePacketRecieveAntiCheatCheck()
		{
			//This prevents the client from disconnecting.
		}

		public override async Task processGameLoop()
		{
			await GameManagerService.Service();
			await base.processGameLoop();
		}

		protected override async Task OnLoginButtonClicked()
		{
			loginMessage1 = "";
			loginMessage2 = "Connecting to server...";
			drawLoginScreen(true);

			PlayerAccountJWTModel model = await HackySharedClientData.Instance.AuthService.TryAuthenticate(new AuthenticationRequestModel("Admin", "Test69!"));
			HackySharedClientData.Instance.AuthButtonListener.UpdatedTokenRepository(model);
			HackySharedClientData.Instance.AuthButtonListener.DispatchAuthenticationResult(model);
		}

		protected override void SendIdlePing()
		{
			//Just stub it out.
		}
	}
}
