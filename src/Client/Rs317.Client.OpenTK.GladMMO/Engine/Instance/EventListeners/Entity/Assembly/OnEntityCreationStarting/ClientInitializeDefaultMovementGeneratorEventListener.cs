using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class ClientInitializeDefaultMovementGeneratorEventListener : BaseSingleEventListenerInitializable<IEntityCreationFinishedEventSubscribable, EntityCreationFinishedEventArgs>
	{
		private IFactoryCreatable<IMovementGenerator<IWorldObject>, EntityAssociatedData<IMovementData>> MovementGeneratorFactory { get; }

		private IEntityGuidMappable<IMovementGenerator<IWorldObject>> MovementGeneratorMappable { get; }

		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		public ClientInitializeDefaultMovementGeneratorEventListener(IEntityCreationFinishedEventSubscribable subscriptionService,
			[NotNull] IEntityGuidMappable<IMovementGenerator<IWorldObject>> movementGeneratorMappable,
			[NotNull] IFactoryCreatable<IMovementGenerator<IWorldObject>, EntityAssociatedData<IMovementData>> movementGeneratorFactory,
			[NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable)
			: base(subscriptionService)
		{
			MovementGeneratorMappable = movementGeneratorMappable ?? throw new ArgumentNullException(nameof(movementGeneratorMappable));
			MovementGeneratorFactory = movementGeneratorFactory ?? throw new ArgumentNullException(nameof(movementGeneratorFactory));
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
		}

		protected override void OnEventFired(object source, EntityCreationFinishedEventArgs args)
		{
			IMovementData movementData = MovementDataMappable.RetrieveEntity(args.EntityGuid);

			IMovementGenerator<IWorldObject> generator = MovementGeneratorFactory.Create(new EntityAssociatedData<IMovementData>(args.EntityGuid, movementData));
			MovementGeneratorMappable.AddObject(args.EntityGuid, generator);
		}
	}
}
