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

		public GladMMOOpenTkClient Client { get; }

		private int CurrentPathIndex = 0;

		public PathMovementGenerator([NotNull] PathBasedMovementData pathData, GladMMOOpenTkClient client)
		{
			MovementData = pathData ?? throw new ArgumentNullException(nameof(pathData));
			Client = client;
		}

		public void Update(IWorldObject entity, long currentTime)
		{
			if (CurrentPathIndex >= MovementData.MovementPath.Count)
				return;

			int xRegion = (int)MovementData.MovementPath[CurrentPathIndex].x / 8;
			int zRegion = (int)MovementData.MovementPath[CurrentPathIndex].z / 8; //current serverside z is y in RS engine.
			int xOffset = (int)MovementData.MovementPath[CurrentPathIndex].x - ((xRegion - 6) * 8);
			int yOffset = (int)MovementData.MovementPath[CurrentPathIndex].z - ((zRegion - 6) * 8);

			int localYOffset = Math.Sign((int)yOffset - (int)entity.CurrentX);
			int localXOffset = Math.Sign((int)xOffset - (int)entity.CurrentY);

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

		public bool isRunning { get; } = true;
	}
}
