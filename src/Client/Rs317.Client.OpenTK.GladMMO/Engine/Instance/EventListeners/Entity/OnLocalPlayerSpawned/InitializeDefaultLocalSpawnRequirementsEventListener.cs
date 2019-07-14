using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeDefaultLocalSpawnRequirementsEventListener : OnLocalPlayerSpawnedEventListener
	{
		private GladMMOOpenTkClient Client { get; }

		public InitializeDefaultLocalSpawnRequirementsEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] GladMMOOpenTkClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			Client.SetWalkableInterfaceId(-1);
		}
	}
}
