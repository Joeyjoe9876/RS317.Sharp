using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using GladMMO;
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

		public DefaultMovementGeneratorFactory([JetBrains.Annotations.NotNull] ILog logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public IMovementGenerator<IWorldObject> Create(EntityAssociatedData<IMovementData> context)
		{
			if(Logger.IsWarnEnabled)
				Logger.Warn($"TODO: Need to implement movement generator.");

			return new MovementGeneratorStub();
		}
	}
}
