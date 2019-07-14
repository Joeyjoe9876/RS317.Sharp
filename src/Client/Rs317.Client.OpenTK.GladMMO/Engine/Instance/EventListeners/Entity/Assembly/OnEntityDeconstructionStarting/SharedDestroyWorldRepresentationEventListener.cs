using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.GladMMO;
using UnityEngine;

namespace GladMMO
{
	/// <summary>
	/// Base shared event listener for EntityDeconstructionStarting that will destroy the
	/// world representation/<see cref="GameObject"/> representation of the Entity.
	/// It will also broadcast the <see cref="IEntityWorldRepresentationDeconstructionFinishedEventSubscribable"/> and
	/// <see cref="IEntityWorldRepresentationDeconstructionStartingEventSubscribable"/> events.
	/// </summary>
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	[AdditionalRegisterationAs(typeof(IEntityWorldRepresentationDeconstructionStartingEventSubscribable))]
	[AdditionalRegisterationAs(typeof(IEntityWorldRepresentationDeconstructionFinishedEventSubscribable))]
	public class SharedDestroyWorldRepresentationEventListener : BaseSingleEventListenerInitializable<IEntityDeconstructionStartingEventSubscribable, EntityDeconstructionStartingEventArgs>, IEntityWorldRepresentationDeconstructionFinishedEventSubscribable, IEntityWorldRepresentationDeconstructionStartingEventSubscribable
	{
		private IReadonlyEntityGuidMappable<IWorldObject> GuidToGameObjectMappable { get; }

		public event EventHandler<EntityWorldRepresentationDeconstructionFinishedEventArgs> OnEntityWorldRepresentationDeconstructionFinished;

		public event EventHandler<EntityWorldRepresentationDeconstructionStartingEventArgs> OnEntityWorldRepresentationDeconstructionStarting;

		public SharedDestroyWorldRepresentationEventListener(IEntityDeconstructionStartingEventSubscribable subscriptionService,
			[NotNull] IReadonlyEntityGuidMappable<IWorldObject> guidToGameObjectMappable)
			: base(subscriptionService)
		{
			GuidToGameObjectMappable = guidToGameObjectMappable ?? throw new ArgumentNullException(nameof(guidToGameObjectMappable));
		}

		protected override void OnEventFired(object source, EntityDeconstructionStartingEventArgs args)
		{
			//Before we even touch the GameObject, let's broadcast this.
			OnEntityWorldRepresentationDeconstructionStarting?.Invoke(this, new EntityWorldRepresentationDeconstructionStartingEventArgs(args.EntityGuid));
			CleanupGameObject(args);
			OnEntityWorldRepresentationDeconstructionFinished?.Invoke(this, new EntityWorldRepresentationDeconstructionFinishedEventArgs(args.EntityGuid));

			//We don't need to touch the IEntityGuidMappable. Those get cleaned up seperately.
		}

		private void CleanupGameObject([NotNull] EntityDeconstructionStartingEventArgs args)
		{
			if(args == null) throw new ArgumentNullException(nameof(args));

			//TODO: Cleanup
		}
	}
}
