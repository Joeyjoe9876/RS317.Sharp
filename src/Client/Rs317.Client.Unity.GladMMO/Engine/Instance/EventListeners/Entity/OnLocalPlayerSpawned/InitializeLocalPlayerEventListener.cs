using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeLocalPlayerEventListener : OnLocalPlayerSpawnedEventListener
	{
		private GladMMOUnityClient Client { get; }

		public InitializeLocalPlayerEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] GladMMOUnityClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			Client.playersObserved[Client.playersObservedCount++] = Client.LOCAL_PLAYER_ID;

			GladMMOUnityClient.localPlayer.visible = true;
			GladMMOUnityClient.localPlayer.lastUpdateTick = GladMMOUnityClient.tick;

			GladMMOUnityClient.localPlayer.standAnimationId = 0x328;
			GladMMOUnityClient.localPlayer.standTurnAnimationId = 0x337;
			GladMMOUnityClient.localPlayer.walkAnimationId = 0x333;
			GladMMOUnityClient.localPlayer.turnAboutAnimationId = 0x334;
			GladMMOUnityClient.localPlayer.turnLeftAnimationId = 0x335;
			GladMMOUnityClient.localPlayer.turnRightAnimationId = 0x336;
			GladMMOUnityClient.localPlayer.runAnimationId = 0x338;

			GladMMOUnityClient.localPlayer.appearanceOffset = 336413342762192;
			GladMMOUnityClient.localPlayer.appearance = new int[12] { 0, 0, 0, 0, 275, 0, 285, 295, 259, 291, 300, 266 };
		}
	}
}
