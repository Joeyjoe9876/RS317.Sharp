using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class RegisterPlayerLevelChangeEventListener : EntityCreationFinishedEventListener
	{
		private IEntityDataChangeCallbackRegisterable DataChangeRegisterable { get; }

		private IReadonlyEntityGuidMappable<IWorldObject> WorldObjectMappable { get; }

		public RegisterPlayerLevelChangeEventListener(IEntityCreationFinishedEventSubscribable subscriptionService,
			[NotNull] IEntityDataChangeCallbackRegisterable dataChangeRegisterable,
			[JetBrains.Annotations.NotNull] IReadonlyEntityGuidMappable<IWorldObject> worldObjectMappable) 
			: base(subscriptionService)
		{
			DataChangeRegisterable = dataChangeRegisterable ?? throw new ArgumentNullException(nameof(dataChangeRegisterable));
			WorldObjectMappable = worldObjectMappable ?? throw new ArgumentNullException(nameof(worldObjectMappable));
		}

		protected override void OnEventFired(object source, EntityCreationFinishedEventArgs args)
		{
			//Only interested in players for now.
			if (args.EntityGuid.EntityType != EntityType.Player)
				return;

			DataChangeRegisterable.RegisterCallback<int>(args.EntityGuid, (int) EUnitFields.UNIT_FIELD_LEVEL, OnEntityLevelChanged);
		}

		private void OnEntityLevelChanged([JetBrains.Annotations.NotNull] NetworkEntityGuid entity, [JetBrains.Annotations.NotNull] EntityDataChangedArgs<int> changeData)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));
			if (changeData == null) throw new ArgumentNullException(nameof(changeData));

			IWorldObject worldObject = WorldObjectMappable.RetrieveEntity(entity);

			worldObject.SetLevel(changeData.NewValue);
		}
	}
}
