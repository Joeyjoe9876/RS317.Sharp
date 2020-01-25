using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeLocalPlayerEventListener : OnLocalPlayerSpawnedEventListener
	{
		private RsUnityClient Client { get; }

		public InitializeLocalPlayerEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] RsUnityClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			Client.playersObserved[Client.playersObservedCount++] = Client.LOCAL_PLAYER_ID;

			RsUnityClient.localPlayer.visible = true;
			RsUnityClient.localPlayer.lastUpdateTick = RsUnityClient.tick;

			RsUnityClient.localPlayer.standAnimationId = 0x328;
			RsUnityClient.localPlayer.standTurnAnimationId = 0x337;
			RsUnityClient.localPlayer.walkAnimationId = 0x333;
			RsUnityClient.localPlayer.turnAboutAnimationId = 0x334;
			RsUnityClient.localPlayer.turnLeftAnimationId = 0x335;
			RsUnityClient.localPlayer.turnRightAnimationId = 0x336;
			RsUnityClient.localPlayer.runAnimationId = 0x338;

			RsUnityClient.localPlayer.appearanceOffset = 336413342762192;
			RsUnityClient.localPlayer.appearance = new int[12] { 0, 0, 0, 0, 275, 0, 285, 295, 259, 291, 300, 266 };
		}
	}
}
