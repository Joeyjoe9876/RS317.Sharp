using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Rs317.Sharp
{
	/// <summary>
	/// This component/<see cref="MonoBehaviour"/> is basically the
	/// main entry point into the RS client.
	/// </summary>
	public sealed class ClientApplication : MonoBehaviour
	{
		//Warning: Future versions of Unity MAY not like never ending awaited Unity3D Awake functions.
		//Think of this like void Main
		private async Task Awake()
		{
			//DO NOT MOVE THIS, YOU WILL CRASH UNITY3D
			Graphics graphics = new Graphics();

			//worldId, portOffset, isMemmbers
			ClientConfiguration configuration = new ClientConfiguration(0, 0, true);

			Unity2DClient client = new Unity2DClient(configuration, graphics);

			try
			{
				Texture2D.allowThreadedTextureCreation = false;

				Task clientRunningAwaitable = signlink.startpriv(IPAddress.Parse("127.0.0.1"), error => UnityEngine.Debug.LogError(error));

				//Wait for signlink
				while(!signlink.IsSignLinkThreadActive)
					await Task.Delay(50)
						.ConfigureAwait(false);

				await clientRunningAwaitable
					.ConfigureAwait(false);
			}
			catch (Exception e)
			{
				UnityEngine.Debug.LogError($"Error: {e.Message}\n\nStack: {e.StackTrace}");
				throw;
			}

			client.createClientFrame(765, 503);
		}
	}
}
