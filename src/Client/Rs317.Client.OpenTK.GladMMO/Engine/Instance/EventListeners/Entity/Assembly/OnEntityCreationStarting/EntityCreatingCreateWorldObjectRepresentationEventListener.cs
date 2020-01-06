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

		public void SetName(string name)
		{

		}

		public void SetLevel(int level)
		{

		}
	}

	//Conceptually this is like a partial factory
	[AdditionalRegisterationAs(typeof(IEntityWorldObjectCreatedEventSubscribable))]
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public class EntityCreatingCreateWorldObjectRepresentationEventListener : BaseSingleEventListenerInitializable<IEntityCreationStartingEventSubscribable, EntityCreationStartingEventArgs>, IEntityWorldObjectCreatedEventSubscribable
	{
		public event EventHandler<EntityWorldObjectCreatedEventArgs> OnEntityWorldObjectCreated;

		private GladMMOOpenTkClient Client { get; }

		private ILocalCharacterDataRepository CharacterDataRepository { get; }

		private IReadonlyEntityGuidMappable<IMovementData> MovementDataMappable { get; }

		public EntityCreatingCreateWorldObjectRepresentationEventListener(IEntityCreationStartingEventSubscribable subscriptionService,
			[NotNull] GladMMOOpenTkClient client,
			[JetBrains.Annotations.NotNull] ILocalCharacterDataRepository characterDataRepository,
			[JetBrains.Annotations.NotNull] IReadonlyEntityGuidMappable<IMovementData> movementDataMappable)
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			CharacterDataRepository = characterDataRepository ?? throw new ArgumentNullException(nameof(characterDataRepository));
			MovementDataMappable = movementDataMappable ?? throw new ArgumentNullException(nameof(movementDataMappable));
		}

		protected override void OnEventFired(object source, EntityCreationStartingEventArgs args)
		{
			if (args.EntityGuid.EntityType == EntityType.Player)
			{
				//TODO: move into a factory.
				if(CharacterDataRepository.CharacterId == args.EntityGuid.EntityId)
					//TODO: Support move than just the local player.
					OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, GladMMOOpenTkClient.localPlayer));
				else
				{
					Client.players[args.EntityGuid.EntityId] = new Player();

					Client.players[args.EntityGuid.EntityId].visible = true;
					Client.players[args.EntityGuid.EntityId].lastUpdateTick = GladMMOOpenTkClient.tick;

					Client.players[args.EntityGuid.EntityId].standAnimationId = 0x328;
					Client.players[args.EntityGuid.EntityId].standTurnAnimationId = 0x337;
					Client.players[args.EntityGuid.EntityId].walkAnimationId = 0x333;
					Client.players[args.EntityGuid.EntityId].turnAboutAnimationId = 0x334;
					Client.players[args.EntityGuid.EntityId].turnLeftAnimationId = 0x335;
					Client.players[args.EntityGuid.EntityId].turnRightAnimationId = 0x336;
					Client.players[args.EntityGuid.EntityId].runAnimationId = 0x338;

					Client.players[args.EntityGuid.EntityId].appearanceOffset = 336413342762192;
					Client.players[args.EntityGuid.EntityId].appearance = new int[12] { 0, 0, 0, 0, 275, 0, 285, 295, 259, 291, 300, 266 };
					Client.players[args.EntityGuid.EntityId].name = "Unknown";

					IMovementData movementData = MovementDataMappable.RetrieveEntity(args.EntityGuid);
					Client.players[args.EntityGuid.EntityId].setPos((int)movementData.InitialPosition.x - Client.baseX, (int)movementData.InitialPosition.z - Client.baseY);

					Client.localPlayers[Client.localPlayerCount++] = args.EntityGuid.EntityId;
					Client.playersObserved[Client.playersObservedCount++] = args.EntityGuid.EntityId;

					OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, Client.players[args.EntityGuid.EntityId]));
				}
			}
			else
				OnEntityWorldObjectCreated?.Invoke(this, new EntityWorldObjectCreatedEventArgs(args.EntityGuid, new WorldObjectStub()));
		}
	}
}