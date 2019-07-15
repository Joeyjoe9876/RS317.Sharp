using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using Rs317.Sharp;

namespace GladMMO
{
	/// <summary>
	/// Event subscriber that links the <see cref="UnityEngine.GameObject"/> and <see cref="NetworkEntityGuid"/> of an Entity
	/// together in a two-way relationship.
	/// </summary>
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public class InitializeEntityWorldMappablesEventListener : BaseSingleEventListenerInitializable<IEntityWorldObjectCreatedEventSubscribable, EntityWorldObjectCreatedEventArgs>
	{
		private IEntityGuidMappable<IWorldObject> GuidToGameObjectMappable { get; }

		public InitializeEntityWorldMappablesEventListener(IEntityWorldObjectCreatedEventSubscribable subscriptionService,
			[NotNull] IEntityGuidMappable<IWorldObject> guidToGameObjectMappable)
			: base(subscriptionService)
		{
			GuidToGameObjectMappable = guidToGameObjectMappable ?? throw new ArgumentNullException(nameof(guidToGameObjectMappable));
		}

		protected override void OnEventFired(object source, EntityWorldObjectCreatedEventArgs args)
		{
			//Basically, all this one does is it just initializes the containers.
			GuidToGameObjectMappable.AddObject(args.EntityGuid, args.WorldReprensetation);
		}
	}
}