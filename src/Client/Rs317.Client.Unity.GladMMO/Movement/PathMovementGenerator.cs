using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using Rs317.GladMMO;
using Rs317.Sharp;
using UnityEngine;

namespace Rs317
{
	public sealed class PathMovementGenerator : IMovementGenerator<IWorldObject>
	{
		private PathBasedMovementData MovementData { get; }

		public GladMMOUnityClient Client { get; }

		private int CurrentPathIndex = 0;

		public PathMovementGenerator([NotNull] PathBasedMovementData pathData, GladMMOUnityClient client)
		{
			MovementData = pathData ?? throw new ArgumentNullException(nameof(pathData));
			Client = client;
		}

		public void Update(IWorldObject entity, long currentTime)
		{
			if (CurrentPathIndex >= MovementData.MovementPath.Count)
				return;

			int xOffset = (int)MovementData.MovementPath[CurrentPathIndex].x - Client.baseX;
			int yOffset = (int)MovementData.MovementPath[CurrentPathIndex].z - Client.baseY;

			int localYOffset = Math.Sign((int)yOffset - (int)entity.CurrentY);
			int localXOffset = Math.Sign((int)xOffset - (int)entity.CurrentX);

			if(localYOffset == 0 && 0 == localXOffset)
			{
				CurrentPathIndex++;
				Update(entity, currentTime);
			}
			else
			{
				entity.setPos(entity.CurrentX + localXOffset, entity.CurrentY + localYOffset);
			}
		}

		public Vector3 CurrentPosition { get; }

		public bool isStarted { get; }

		public bool isFinished { get; }

		public bool isRunning { get; } = true;
	}
}
