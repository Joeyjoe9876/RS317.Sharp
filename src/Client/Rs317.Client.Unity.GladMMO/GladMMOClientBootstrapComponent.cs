﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Glader.Essentials;
using GladMMO;
using Rs317.GladMMMO;
using Rs317.GladMMO;

namespace Rs317.Sharp
{
	public static class GladMMOProgram
	{
		public static RsUnityClient RootClient { get; set; }

		public static GameManager RootGameManager { get; set; } = new GameManager();
	}

	/// <summary>
	/// Bootstrap client component used specifically for GladMMO builds.
	/// </summary>
	public sealed class GladMMOClientBootstrapComponent : ClientBootstrapComponent
	{
		protected override async Task Start()
		{
			//WebGL doesn't support ConfigureAwait: https://issuetracker.unity3d.com/issues/webgl-task-awaited-with-task-dot-configureawait-false-does-not-continue-in-a-webgl-build
			if (RsUnityPlatform.isWebGLBuild)
			{
				//Important to set these before the IoC containers are created. SO ASAP
				//otherwise they'll use the wrong stuff.
				AuthenticationDependencyAutofacModule.HttpClientHandlerFactory = () => new WebGLHttpClientHandler();
				ServiceDiscoveryDependencyAutofacModule.HttpClientHandlerFactory = () => new WebGLHttpClientHandler();
				ZoneServerServiceDependencyAutofacModule.HttpClientHandlerFactory = () => new WebGLHttpClientHandler();

				//{ "endpoint":{ "endpointAddress":"http://72.190.177.214","endpointPort":5012},"resultCode":1}
				ZoneServerServiceDependencyAutofacModule.PrecomputedEndpoint = @"http://72.190.177.214:5012/";

				//{"endpoint":{"endpointAddress":"http://test-guardians-auth.azurewebsites.net","endpointPort":80},"resultCode":1}
				AuthenticationDependencyAutofacModule.PrecomputedEndpoint = @"http://test-guardians-auth.azurewebsites.net:80/";

				GladMMOAsyncSettings.ConfigureAwaitFalseSupported = false;
			}

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
			if(RsUnityPlatform.isWebGLBuild)
			{
				//Used for Task.Delay in WebGL (Task.Delay doesn't work in WebGL directly)
				WebGLUnityTaskDelayFactory delayFactory = new UnityEngine.GameObject("Task Delayer").AddComponent<WebGLUnityTaskDelayFactory>();

				if(RsUnityPlatform.isInEditor)
				{
					GladMMOProgram.RootClient = new GladMMORsUnityWebGLClient(configuration, GraphicsObject, this, new DefaultWebSocketClientFactory(), delayFactory, GladMMOProgram.RootGameManager);
				}
				else
				{
					GladMMOProgram.RootClient = new GladMMORsUnityWebGLClient(configuration, GraphicsObject, this, new WebGLWebSocketFactory(delayFactory), delayFactory, GladMMOProgram.RootGameManager);
				}
			}
			else
			{
				GladMMOUnityClient client = new GladMMOUnityClient(configuration, GraphicsObject, GladMMOProgram.RootGameManager);
				GladMMOProgram.RootClient = client;
			}

			return GladMMOProgram.RootClient;
		}
	}
}
