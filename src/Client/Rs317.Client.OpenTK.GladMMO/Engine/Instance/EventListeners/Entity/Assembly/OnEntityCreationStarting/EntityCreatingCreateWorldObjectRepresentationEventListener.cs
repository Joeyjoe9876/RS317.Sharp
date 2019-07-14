using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using UnityEngine;

namespace GladMMO
{
	public sealed class WorldObjectStub : IWorldObject
	{

	}

	//Conceptually this is like a partial factory
	[AdditionalRegisterationAs(typeof(IEntityWorldObjectCreatedEventSubscribable))]
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public class EntityCreatingCreateWorldObjectRepresentationEventListener : BaseSingleEventListenerInitializable<IEntityCreationStartingEventSubscribable, EntityCreationStartingEventArgs>, IEntityWorldObjectCreatedEventSubscribable
	{
		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		public event EventHandler<EntityWorldObjectCreatedEventArgs> OnEntityWorldObjectCreated;

		public EntityCreatingCreateWorldObjectRepresentationEventListener(IEntityCreationStartingEventSubscribable subscriptionService,
			[NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable)
			: base(subscriptionService)
		{
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
		}

		protected override void OnEventFired(object source, EntityCreationStartingEventArgs args)
		{
			OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, new WorldObjectStub()));
		}
	}
}