using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeLocalPlayerEventListener : OnLocalPlayerSpawnedEventListener
	{
		private GladMMOOpenTkClient Client { get; }

		public InitializeLocalPlayerEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] GladMMOOpenTkClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			Client.playersObserved[Client.playersObservedCount++] = Client.LOCAL_PLAYER_ID;

			//TODO: This SHOULD be coming from the initial position of the movement data.
			GladMMOOpenTkClient.localPlayer.setPos(50, 50, true);

			GladMMOOpenTkClient.localPlayer.visible = true;
			GladMMOOpenTkClient.localPlayer.lastUpdateTick = GladMMOOpenTkClient.tick;

			GladMMOOpenTkClient.localPlayer.standAnimationId = 0x328;
			GladMMOOpenTkClient.localPlayer.standTurnAnimationId = 0x337;
			GladMMOOpenTkClient.localPlayer.walkAnimationId = 0x333;
			GladMMOOpenTkClient.localPlayer.turnAboutAnimationId = 0x334;
			GladMMOOpenTkClient.localPlayer.turnLeftAnimationId = 0x335;
			GladMMOOpenTkClient.localPlayer.turnRightAnimationId = 0x336;
			GladMMOOpenTkClient.localPlayer.runAnimationId = 0x338;

			GladMMOOpenTkClient.localPlayer.appearanceOffset = 336413342762192;
			GladMMOOpenTkClient.localPlayer.appearance = new int[12] { 0, 0, 0, 0, 275, 0, 285, 295, 259, 291, 300, 266 };


			/*	playerProperties.put(DataType.SHORT, 0x328); // stand
				playerProperties.put(DataType.SHORT, 0x337); // stand turn
				playerProperties.put(DataType.SHORT, 0x333); // walk
				playerProperties.put(DataType.SHORT, 0x334); // turn 180
				playerProperties.put(DataType.SHORT, 0x335); // turn 90 cw
				playerProperties.put(DataType.SHORT, 0x336); // turn 90 ccw
				playerProperties.put(DataType.SHORT, 0x338); // run*/

			Client.playerPositionY = 51;
			Client.playerPositionX = 54;

			//updatePlayers(packetSize, inStream);
			Client.loadingMap = false;
		}
	}
}
