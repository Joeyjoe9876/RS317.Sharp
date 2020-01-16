using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using Rs317.GladMMO;

namespace Rs317.Sharp
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class LogDisconnectionEventListener : BaseSingleEventListenerInitializable<INetworkClientDisconnectedEventSubscribable>
	{
		private ILog Logger { get; }

		public LogDisconnectionEventListener(INetworkClientDisconnectedEventSubscribable subscriptionService, ILog logger) : base(subscriptionService)
		{
			Console.WriteLine($"Created LogDisconnectionEventListener");
			Logger = logger;
		}

		protected override void OnEventFired(object source, EventArgs args)
		{
			if(Logger.IsInfoEnabled)
				Logger.Info($"Network client disconnected.");
		}
	}
}
