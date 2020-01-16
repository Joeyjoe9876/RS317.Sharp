using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using Rs317.GladMMO;

namespace Rs317.Sharp
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class DespawnRunescapePlayerEventListener : BaseSingleEventListenerInitializable<IEntityDeconstructionFinishedEventSubscribable, EntityDeconstructionFinishedEventArgs>
	{
		private RsUnityClient Client { get; }

		private ILog Logger { get; }

		public DespawnRunescapePlayerEventListener(IEntityDeconstructionFinishedEventSubscribable subscriptionService, [JetBrains.Annotations.NotNull] RsUnityClient client,
			[JetBrains.Annotations.NotNull] ILog logger) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public DespawnRunescapePlayerEventListener(IEntityDeconstructionFinishedEventSubscribable subscriptionService, ILog logger) 
			: base(subscriptionService)
		{
			Logger = logger;
		}

		protected override void OnEventFired(object source, EntityDeconstructionFinishedEventArgs args)
		{
			if (args.EntityGuid.EntityType != EntityType.Player)
				return;

			if(Logger.IsInfoEnabled)
				Logger.Info($"Nulling despawned player.");

			//RS client just sets a player reference to null when it's suppose to despawn
			//It's stupid, but it can't be helped. It's what Jagex did.
			Client.players[args.EntityGuid.EntityId] = null;

			//Internally the RS client will track observed and local
			//player count as well as update based on PID in the local
			//mapping collections (local players, playersobserved and etc)
			//so the pid must be removed and other elments shifted downard
			//while also decrementing the count
			//This prevents the multiple update ticks per entity
			//if they relog or something.
			Client.localPlayerCount--;
			Client.playersObservedCount--;
			Client.localPlayers.Remove(args.EntityGuid.EntityId);
			Client.playersObserved.Remove(args.EntityGuid.EntityId);
		}
	}
}
