using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using Rs317.Sharp;
using UnityEngine;

namespace GladMMO
{
	public sealed class WorldObjectStub : IWorldObject
	{
		public int CurrentX { get; private set; }

		public int CurrentY { get; private set; }

		public void setPos(int x, int y)
		{
			CurrentX = x;
			CurrentY = y;
		}
	}

	//Conceptually this is like a partial factory
	[AdditionalRegisterationAs(typeof(IEntityWorldObjectCreatedEventSubscribable))]
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public class EntityCreatingCreateWorldObjectRepresentationEventListener : BaseSingleEventListenerInitializable<IEntityCreationStartingEventSubscribable, EntityCreationStartingEventArgs>, IEntityWorldObjectCreatedEventSubscribable
	{
		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		public event EventHandler<EntityWorldObjectCreatedEventArgs> OnEntityWorldObjectCreated;

		private GladMMOOpenTkClient Client { get; }

		public EntityCreatingCreateWorldObjectRepresentationEventListener(IEntityCreationStartingEventSubscribable subscriptionService,
			[NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable,
			[NotNull] GladMMOOpenTkClient client)
			: base(subscriptionService)
		{
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnEventFired(object source, EntityCreationStartingEventArgs args)
		{
			if (args.EntityGuid.EntityType == EntityType.Player)
			{
				//TODO: Support move than just the local player.
				OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, GladMMOOpenTkClient.localPlayer));
			}
			else
				OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, new WorldObjectStub()));
		}
	}
}