using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Glader.Essentials;
using Rs317.GladMMMO;
using Rs317.GladMMO;

namespace Rs317.Sharp
{
	public static class GladMMOProgram
	{
		public static GladMMOUnityClient RootClient { get; set; }

		public static GameManager RootGameManager { get; set; } = new GameManager();
	}

	/// <summary>
	/// Bootstrap client component used specifically for GladMMO builds.
	/// </summary>
	public sealed class GladMMOClientBootstrapComponent : ClientBootstrapComponent
	{
		protected override async Task Start()
		{
			//Wait for base engine start
			await base.Start();

			//Now we can do the GladMMO specific.

			//Dev hack to enable HTTPS for now.
			ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.CheckCertificateRevocationList = false;

			Console.WriteLine($"RS2 user client - release #{317} using Rs317.Sharp.Extended by Glader");

			//This starts us at the login screen.
			GladMMOProgram.RootGameManager.OnLoginStateChanged(this, new HookableVariableValueChangedEventArgs<bool>(false, false));
		}

		protected override RsUnityClient CreateRsClient(ClientConfiguration configuration)
		{
			GladMMOUnityClient client = new GladMMOUnityClient(configuration, GraphicsObject, GladMMOProgram.RootGameManager);
			GladMMOProgram.RootClient = client;
			return client;
		}
	}
}
