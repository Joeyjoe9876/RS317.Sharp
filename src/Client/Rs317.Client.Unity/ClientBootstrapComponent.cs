using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	/// <summary>
	/// This is the component that should be attached in the scene.
	/// It's what actually starts the entire RSClient within Unity3D.
	/// </summary>
	public sealed class ClientBootstrapComponent : MonoBehaviour
	{
		[SerializeField]
		private UnityRsGraphics GraphicsObject;

		//Called on scene start, which starts the underlying client.
		private async Task Start()
		{
			//Important for cross-thread interaction for creating "images".
			Texture.allowThreadedTextureCreation = true;
			UnitySystemConsoleRedirector.Redirect();
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => Debug.LogError($"Unhandled Exception: {args.ExceptionObject.ToString()}");

			//765, 503 default size.
			Screen.SetResolution(765, 503, Screen.fullScreenMode);

			try
			{
				await StartClient(0, 0, true);
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to start client. Reason: {e.ToString()}");
				throw;
			}
		}

		private async Task StartClient(int localWorldId, short portOffset, bool membersWorld)
		{
			Debug.Log($"Starting client.");

			Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"));
			ClientConfiguration configuration = new ClientConfiguration(localWorldId, portOffset, membersWorld);

			//Wait for signlink
			while(!signlink.IsSignLinkThreadActive)
				await Task.Delay(50)
					.ConfigureAwait(true);

			Debug.Log($"Signlink started.");

			RsUnityClient client1 = new RsUnityClient(configuration, GraphicsObject);
			GraphicsObject.GameStateHookable = client1;
			//windowsFormApplication.RegisterInputSubscriber(client1);
			client1.createClientFrame(765, 503);
			

			Debug.Log($"Client frame created.");
		}
	}
}
