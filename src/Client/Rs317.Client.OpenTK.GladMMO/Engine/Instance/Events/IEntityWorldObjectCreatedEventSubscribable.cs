using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;

namespace Rs317.GladMMO
{
	public interface IEntityWorldObjectCreatedEventSubscribable
	{
		event EventHandler<EntityWorldObjectCreatedEventArgs> OnEntityWorldObjectCreated;
	}

	public sealed class EntityWorldObjectCreatedEventArgs : EventArgs
	{
		public NetworkEntityGuid EntityGuid { get; }

		public IWorldObject WorldReprensetation { get; }

		public EntityWorldObjectCreatedEventArgs([NotNull] NetworkEntityGuid entityGuid,
			[NotNull] IWorldObject worldReprensetation)
		{
			EntityGuid = entityGuid ?? throw new ArgumentNullException(nameof(entityGuid));
			WorldReprensetation = worldReprensetation ?? throw new ArgumentNullException(nameof(worldReprensetation));
		}

		public EntityWorldObjectCreatedEventArgs(IWorldObject worldReprensetation)
		{
			WorldReprensetation = worldReprensetation;
		}
	}
}
