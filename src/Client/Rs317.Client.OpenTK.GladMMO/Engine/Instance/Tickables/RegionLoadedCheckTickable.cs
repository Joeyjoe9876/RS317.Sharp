using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using GladMMO;

namespace Rs317.GladMMO
{
	//This system basically checks to see if we're outside the bounds
	//of the currently loaded region and loads the next one instead.
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class RegionLoadedCheckTickable : IGameFixedTickable
	{
		private GladMMOOpenTkClient Client { get; }

		public RegionLoadedCheckTickable([JetBrains.Annotations.NotNull] GladMMOOpenTkClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public void OnGameFixedTick()
		{
			if (!GladMMOOpenTkClient.localPlayer.visible)
				return;

			int regionX = (Client.baseX + 52) / 8 + 6;
			int regionY = (Client.baseY + 52) / 8 + 6;

			//Now compare to current position
			int currentRegionX = (Client.baseX + GladMMOOpenTkClient.localPlayer.waypointX[0]) / 8 + 6;
			int currentRegionY = (Client.baseY + GladMMOOpenTkClient.localPlayer.waypointY[0]) / 8 + 6;

			//If we're more than 2 region chunks away from the current base
			if (Math.Abs(currentRegionX - regionX) > 2 || Math.Abs(currentRegionY - regionY) > 2)
			{
				//Then we should set the new region
				int newRegionX = (Client.baseX + GladMMOOpenTkClient.localPlayer.waypointX[0]) / 8;
				int newRegionY = (Client.baseY + GladMMOOpenTkClient.localPlayer.waypointY[0]) / 8;

				Client.InitializePlayerRegion(newRegionX, newRegionY);

				//For some reason, client will go into Loading - Please Wait until Packet81 is sent
				//Packet81 is the next update packet.
				//To avoid this endless hang, we can just indicate that the map loading is done. As the update packet does.
				Client.loadingMap = false;
			}
		}
	}
}
