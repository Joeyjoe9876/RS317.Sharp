using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using Rs317.Sharp;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class ClientInitializeDefaultMovementGeneratorEventListener : BaseSingleEventListenerInitializable<IEntityWorldObjectCreatedEventSubscribable, EntityWorldObjectCreatedEventArgs>
	{
		private IFactoryCreatable<IMovementGenerator<IWorldObject>, EntityAssociatedData<IMovementData>> MovementGeneratorFactory { get; }

		private IEntityGuidMappable<IMovementGenerator<IWorldObject>> MovementGeneratorMappable { get; }

		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		private RsUnityClient Client { get; }

		private IReadonlyNetworkTimeService TimeService { get; }

		public ClientInitializeDefaultMovementGeneratorEventListener(IEntityWorldObjectCreatedEventSubscribable subscriptionService,
			[NotNull] IEntityGuidMappable<IMovementGenerator<IWorldObject>> movementGeneratorMappable,
			[NotNull] IFactoryCreatable<IMovementGenerator<IWorldObject>, EntityAssociatedData<IMovementData>> movementGeneratorFactory,
			[NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable,
			[JetBrains.Annotations.NotNull] RsUnityClient client,
			[JetBrains.Annotations.NotNull] IReadonlyNetworkTimeService timeService)
			: base(subscriptionService)
		{
			MovementGeneratorMappable = movementGeneratorMappable ?? throw new ArgumentNullException(nameof(movementGeneratorMappable));
			MovementGeneratorFactory = movementGeneratorFactory ?? throw new ArgumentNullException(nameof(movementGeneratorFactory));
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
			Client = client ?? throw new ArgumentNullException(nameof(client));
			TimeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
		}

		protected override void OnEventFired(object source, EntityWorldObjectCreatedEventArgs args)
		{
			IMovementData movementData = MovementDataMappable.RetrieveEntity(args.EntityGuid);

			IMovementGenerator<IWorldObject> generator = MovementGeneratorFactory.Create(new EntityAssociatedData<IMovementData>(args.EntityGuid, movementData));
			MovementGeneratorMappable.AddObject(args.EntityGuid, generator);

			InitializePosition(args.EntityGuid, movementData, generator, args.WorldReprensetation);
		}

		private void InitializePosition([JetBrains.Annotations.NotNull] NetworkEntityGuid guid, 
			[JetBrains.Annotations.NotNull] IMovementData movementData, 
			[JetBrains.Annotations.NotNull] IMovementGenerator<IWorldObject> movementGenerator, 
			[JetBrains.Annotations.NotNull] IWorldObject entityWorldObject)
		{
			if (guid == null) throw new ArgumentNullException(nameof(guid));
			if (movementData == null) throw new ArgumentNullException(nameof(movementData));
			if (movementGenerator == null) throw new ArgumentNullException(nameof(movementGenerator));
			if (entityWorldObject == null) throw new ArgumentNullException(nameof(entityWorldObject));

			//We need to fast forward the fake/stub WorldObject so that we can get an accurate initial position.
			if(movementData is PathBasedMovementData)
			{
				IWorldObject worldObjectStub = new WorldObjectStub((int)movementData.InitialPosition.x - Client.baseX, (int)movementData.InitialPosition.z - Client.baseY);

				//At this point, the worldObjectStub actually will have the correct initial position
				movementGenerator.Update(worldObjectStub, TimeService.CurrentRemoteTime);
				entityWorldObject.DirectSetPosition(worldObjectStub.CurrentX, worldObjectStub.CurrentY);
			}
			else
				entityWorldObject.setPos((int)movementData.InitialPosition.x - Client.baseX, (int)movementData.InitialPosition.z - Client.baseY);
		}
	}
}
