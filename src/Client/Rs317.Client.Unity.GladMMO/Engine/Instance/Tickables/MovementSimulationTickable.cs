using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using Rs317.Sharp;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class MovementSimulationTickable : IGameFixedTickable
	{
		private IReadonlyEntityGuidMappable<IMovementGenerator<IWorldObject>> MovementGenerators { get; }

		private IReadonlyEntityGuidMappable<IWorldObject> WorldObjectMap { get; }

		private IReadonlyNetworkTimeService TimeService { get; }

		private IReadonlyKnownEntitySet KnonwnEntities { get; }

		private RsUnityClient Client { get; }

		/// <inheritdoc />
		public MovementSimulationTickable(
			IReadonlyEntityGuidMappable<IMovementGenerator<IWorldObject>> movementGenerators,
			IReadonlyEntityGuidMappable<IWorldObject> worldObjectMap,
			INetworkTimeService timeService,
			[NotNull] IReadonlyKnownEntitySet knonwnEntities,
			[JetBrains.Annotations.NotNull] RsUnityClient client)
		{
			MovementGenerators = movementGenerators ?? throw new ArgumentNullException(nameof(movementGenerators));
			WorldObjectMap = worldObjectMap ?? throw new ArgumentNullException(nameof(worldObjectMap));
			TimeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
			KnonwnEntities = knonwnEntities ?? throw new ArgumentNullException(nameof(knonwnEntities));
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		/// <inheritdoc />
		public void OnGameFixedTick()
		{
			foreach (var entry in MovementGenerators.EnumerateWithGuid(KnonwnEntities))
			{
				IWorldObject worldObject = WorldObjectMap.RetrieveEntity(entry.EntityGuid);
				worldObject.SetLastUpdateTick(Client.CurrentTick);
				entry.ComponentValue.Update(worldObject, TimeService.CurrentRemoteTime);
			}
		}
	}
}
