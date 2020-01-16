using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using Rs317.GladMMO;
using Rs317.Sharp;
using UnityEngine;

namespace Rs317
{
	public sealed class PathMovementGenerator : Rs317.Sharp.BaseMovementGenerator<PathBasedMovementData>
	{
		public RsUnityClient Client { get; }

		private int CurrentPathIndex = 0;

		public PathMovementGenerator([NotNull] PathBasedMovementData pathData, [NotNull] RsUnityClient client)
			: base(pathData)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override Vector3 Start(IWorldObject entity, long currentTime)
		{
			long diff = currentTime - MovementData.TimeStamp;

			if(diff <= 0)
				return ComputeCurrentPosition(entity);

			//Guard against short paths.
			if (MovementData.MovementPath.Count < 1)
			{
				StopGenerator();
				return ComputeCurrentPosition(entity);
			}

			//Now we need to find out if we need to tick the path
			//forward afew ticks if the diff is bigger than a tick

			//the reason we double this is because we want to save some 600 ms tick diff for the actual update.
			//After Start is called the InternalUpdate will be called by the root caller
			while(diff >= RsClientConstants.RsClientTicksInTicks)
			{
				//only teleport if we're afew ticks behind.
				TickPathForward(entity, currentTime, diff >= RsClientConstants.RsClientTicksInTicks * 2);

				//The TickForward handles stopping when it needs to.
				if (isFinished)
					return ComputeCurrentPosition(entity);

				//Subtract a tick, then we'll potentially repeat this.
				diff -= RsClientConstants.RsClientTicksInTicks;
			}

			return ComputeCurrentPosition(entity);
		}

		public override void Update(IWorldObject entity, long currentTime)
		{
			//it's possible after starting that it has finally become finished.
			//I encountered this with clientside pathing from Runescape emulation lol.
			if(isFinished)
				return;

			if (!isStarted)
			{
				CurrentPosition = Start(entity, currentTime);
				isStarted = true;
			}
			else
			{
				//It was just easier for this RS path generator to not update after Start.
				CurrentPosition = InternalUpdate(entity, currentTime); //don't update if we called Start
			}
		}

		private bool isPathEndReached()
		{
			return (CurrentPathIndex) >= (MovementData.MovementPath.Count - 1);
		}

		private static Vector3 ComputeCurrentPosition(IWorldObject entity)
		{
			return new Vector3(entity.CurrentX, 0.0f, entity.CurrentY);
		}

		protected override Vector3 InternalUpdate(IWorldObject entity, long currentTime)
		{
			if(isFinished)
				return new Vector3(entity.CurrentX, 0.0f, entity.CurrentY);

			TickPathForward(entity, currentTime);

			return new Vector3(entity.CurrentX, 0.0f, entity.CurrentY);
		}

		private void TickPathForward(IWorldObject entity, long currentTime, bool instant = false)
		{
			int xOffset = (int) MovementData.MovementPath[CurrentPathIndex].x - Client.baseX;
			int yOffset = (int) MovementData.MovementPath[CurrentPathIndex].z - Client.baseY;

			int localYOffset = Math.Sign((int) yOffset - (int) entity.CurrentY);
			int localXOffset = Math.Sign((int) xOffset - (int) entity.CurrentX);

			if (localYOffset == 0 && 0 == localXOffset)
			{
				//If we have no Y or X diff this tick and we're on the final path
				//point then it's over.
				if (isPathEndReached())
				{
					StopGenerator();
					return;
				}

				CurrentPathIndex++;
				InternalUpdate(entity, currentTime);
			}
			else
			{
				if(instant)
					entity.DirectSetPosition(entity.CurrentX + localXOffset, entity.CurrentY + localYOffset);
				else
					entity.setPos(entity.CurrentX + localXOffset, entity.CurrentY + localYOffset);
			}
		}
	}
}
