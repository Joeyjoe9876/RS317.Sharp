using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeClientLoadingPositionEventListener : OnLocalPlayerSpawnedEventListener
	{
		private GladMMOOpenTkClient Client { get; }

		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		public InitializeClientLoadingPositionEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] GladMMOOpenTkClient client,
			[NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			//Client region will be the x,z coordinates of the initial movement data
			//divided by 8 + 6.
			//x, z / 8 + 6
			IMovementData movementData = MovementDataMappable.RetrieveEntity(args.EntityGuid);

			int xRegion = (int)movementData.InitialPosition.x / 8;
			int zRegion = (int)movementData.InitialPosition.z / 8; //current serverside z is y in RS engine.
			int xOffset = (int)movementData.InitialPosition.x - ((xRegion - 6) * 8);
			int yOffset = (int)movementData.InitialPosition.z - ((zRegion - 6) * 8);

			Client.InitializePlayerRegion(xRegion, zRegion);
			
			//The offset from the region should be used in setPosition
			//until an absolute method is available
			GladMMOOpenTkClient.localPlayer.setPos(xOffset, yOffset, true);
			Client.loadingMap = false;
		}
	}
}
