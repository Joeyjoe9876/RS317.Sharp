using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Glader.Essentials;
using GladMMO;
using Rs317.GladMMO;
using UnityEngine.SceneManagement;

namespace Rs317.Sharp
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class LogOutRunescapeClientOnDisconnectEventListener : BaseSingleEventListenerInitializable<INetworkClientDisconnectedEventSubscribable>
	{
		private IGameContextEventQueueable ContextQueueable { get; }

		private RsUnityClient Client { get; }

		public LogOutRunescapeClientOnDisconnectEventListener(INetworkClientDisconnectedEventSubscribable subscriptionService,
			[JetBrains.Annotations.NotNull] IGameContextEventQueueable contextQueueable,
			[JetBrains.Annotations.NotNull] RsUnityClient client) 
			: base(subscriptionService)
		{
			Console.WriteLine($"Created LogOutRunescapeClientOnDisconnectEventListener");
			ContextQueueable = contextQueueable ?? throw new ArgumentNullException(nameof(contextQueueable));
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnEventFired(object source, EventArgs args)
		{
			//Just queue up a logout for the client.
			ContextQueueable.Enqueue(Client.logout);
		}
	}
}
