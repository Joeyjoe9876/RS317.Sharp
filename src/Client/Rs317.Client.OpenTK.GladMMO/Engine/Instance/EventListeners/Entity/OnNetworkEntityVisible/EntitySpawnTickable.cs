using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using Glader.Essentials;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class EntitySpawnTickable : SharedEntitySpawnTickable<INetworkEntityVisibleEventSubscribable, NetworkEntityNowVisibleEventArgs>
	{
		public EntitySpawnTickable(INetworkEntityVisibleEventSubscribable subscriptionService, ILog logger, IKnownEntitySet knownEntities) 
			: base(subscriptionService, logger, knownEntities)
		{

		}
	}
}
