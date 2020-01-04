using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using GladMMO.Client;
using Rs317.GladMMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public class GladMMOProgram
	{
		public static GladMMOOpenTkClient RootClient { get; set; }

		public static GameManager RootGameManager { get; set; } = new GameManager();

		public static async Task Main(string[] args)
		{
			var clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"))
				.ConfigureAwait(false);

			//Wait for signlink
			while(!signlink.IsSignLinkThreadActive)
				await Task.Delay(50)
					.ConfigureAwait(false);

			//Dev hack to enable HTTPS for now.
			ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.CheckCertificateRevocationList = false;

			Console.WriteLine($"RS2 user client - release #{317} using Rs317.Sharp.Extended by Glader");

			using (var scope = GameManager.BuildTitleScreenContainer())
			{
				GameManager.LifetimeDependencyScope = scope;

				foreach(var i in scope.Resolve<IEnumerable<IGameInitializable>>())
					await i.OnGameInitialized();

				using(OpenTKGameWindow gameWindow = new OpenTKGameWindow(765, 503, scope.Resolve<IInputCallbackSubscriber>(), scope.Resolve<IImagePaintEventPublisher>(), scope.Resolve<IGameStateHookable>()))
				{
					//Client creates its own thread behind the scenes
					RootClient.createClientFrame(765, 503);

					//Main thread here is blocked as render/input thread (OpenTK/OpenGL) runs.
					gameWindow.Run(20, 60);
				}
			}
		}
	}
}
