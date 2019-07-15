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
using Rs317.GladMMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class GladMMOOpenTkClient : OpenTKClient
	{
		public GameManager GameManagerService { get; }

		public event EventHandler OnLoginButtonClickedEvent;

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
	}
}
