using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using GladMMO;
using Rs317.Sharp;
using UnityEngine;

namespace Rs317.GladMMO
{
	public class MovementGeneratorStub : IMovementGenerator<IWorldObject>
	{
		public void Update(IWorldObject entity, long currentTime)
		{
		}

		public Vector3 CurrentPosition { get; }

		public bool isRunning { get; }
	}

	public sealed class DefaultMovementGeneratorFactory : IFactoryCreatable<IMovementGenerator<IWorldObject>, EntityAssociatedData<IMovementData>>
	{
		private ILog Logger { get; }

		private GladMMOOpenTkClient Client { get; }

		public DefaultMovementGeneratorFactory([JetBrains.Annotations.NotNull] ILog logger, [JetBrains.Annotations.NotNull] GladMMOOpenTkClient client)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public IMovementGenerator<IWorldObject> Create(EntityAssociatedData<IMovementData> context)
		{
			if (context.Data is PathBasedMovementData pd)
			{
				return new PathMovementGenerator(pd, Client);
			}
			else
			{
				if(Logger.IsWarnEnabled)
					Logger.Warn($"TODO: Need to implement movement generator.");

				return new MovementGeneratorStub();
			}
		}
	}
}
