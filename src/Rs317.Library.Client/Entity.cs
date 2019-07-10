using System;

namespace Rs317.Sharp
{
	public class Entity : Animable
	{
		public int entScreenX;

		public int entScreenY;

		public int index = -1;

		public int[] waypointX;

		public int[] waypointY;
		public int interactingEntity;
		public int stepsDelayed { get; set; }

		public int degreesToTurn { get; protected set; }

		public int runAnimationId { get; protected set; }

		public String overheadTextMessage;
		public int height;
		public int turnDirection;
		public int standAnimationId { get; set; }
		public int standTurnAnimationId { get; set; }
		public int chatColour { get; set; }
		public int[] hitArray { get; private set; }
		public int[] hitMarkTypes { get; private set; }
		public int[] hitsLoopCycle { get; private set; }
		public int queuedAnimationId { get; set; }
		public int queuedAnimationFrame { get; set; }
		public int queuedAnimationDuration;
		public int graphicId { get; set; }
		public int currentAnimationId { get; set; }
		public int currentAnimationTimeRemaining { get; set; }
		public int graphicEndCycle { get; set; }
		public int graphicHeight { get; set; }
		public int waypointCount { get; set; }
		public int animation;
		public int currentAnimationFrame { get; set; }
		public int currentAnimationDuration { get; set; }
		public int animationDelay { get; set; }
		public int currentAnimationLoopCount { get; set; }
		public int chatEffect { get; set; }
		public int loopCycleStatus;
		public int currentHealth;
		public int maxHealth;
		public int textCycle { get; set; }
		public int lastUpdateTick { get; set; }
		public int faceTowardX { get; set; }
		public int faceTowardY { get; set; }
		public int boundaryDimension { get; protected set; }
		public bool dynamic { get; set; }
		public int stepsRemaining { get; set; }
		public int startX { get; set; }
		public int endX { get; set; }
		public int startY { get; set; }
		public int endY { get; set; }
		public int tickStart { get; set; }
		public int tickEnd { get; set; }
		public int direction { get; set; }
		public int x;
		public int y;
		public int currentRotation { get; set; }
		public bool[] waypointRan { get; private set; }
		public int walkAnimationId { get; set; }
		public int turnAboutAnimationId { get; set; }
		public int turnLeftAnimationId { get; set; }
		public int turnRightAnimationId { get; set; }

		protected Entity()
		{
			waypointX = new int[10];
			waypointY = new int[10];
			interactingEntity = -1;
			degreesToTurn = 32;
			runAnimationId = -1;
			height = 200;
			standAnimationId = -1;
			standTurnAnimationId = -1;
			hitArray = new int[4];
			hitMarkTypes = new int[4];
			hitsLoopCycle = new int[4];
			queuedAnimationId = -1;
			graphicId = -1;
			animation = -1;
			loopCycleStatus = -1000;
			textCycle = 100;
			boundaryDimension = 1;
			dynamic = false;
			waypointRan = new bool[10];
			walkAnimationId = -1;
			turnAboutAnimationId = -1;
			turnRightAnimationId = -1;
			turnLeftAnimationId = -1;
		}

		public virtual bool isVisible()
		{
			return false;
		}

		public void move(bool flag, int direction)
		{
			int x = waypointX[0];
			int y = waypointY[0];
			if(direction == 0)
			{
				x--;
				y++;
			}

			if(direction == 1)
				y++;
			if(direction == 2)
			{
				x++;
				y++;
			}

			if(direction == 3)
				x--;
			if(direction == 4)
				x++;
			if(direction == 5)
			{
				x--;
				y--;
			}

			if(direction == 6)
				y--;
			if(direction == 7)
			{
				x++;
				y--;
			}

			if(animation != -1 && AnimationSequence.animations[animation].precedenceWalking == 1)
				animation = -1;
			if(waypointCount < 9)
				waypointCount++;
			for(int l = waypointCount; l > 0; l--)
			{
				waypointX[l] = waypointX[l - 1];
				waypointY[l] = waypointY[l - 1];
				waypointRan[l] = waypointRan[l - 1];
			}

			waypointX[0] = x;
			waypointY[0] = y;
			waypointRan[0] = flag;
		}

		public void resetPath()
		{
			waypointCount = 0;
			stepsRemaining = 0;
		}

		public void setPos(int x, int y, bool teleported)
		{
			if(animation != -1 && AnimationSequence.animations[animation].precedenceWalking == 1)
				animation = -1;
			if(!teleported)
			{
				int distanceX = x - waypointX[0];
				int distanceY = y - waypointY[0];
				if(distanceX >= -8 && distanceX <= 8 && distanceY >= -8 && distanceY <= 8)
				{
					if(waypointCount < 9)
						waypointCount++;
					for(int waypoint = waypointCount; waypoint > 0; waypoint--)
					{
						waypointX[waypoint] = waypointX[waypoint - 1];
						waypointY[waypoint] = waypointY[waypoint - 1];
						waypointRan[waypoint] = waypointRan[waypoint - 1];
					}

					waypointX[0] = x;
					waypointY[0] = y;
					waypointRan[0] = false;
					return;
				}
			}

			waypointCount = 0;
			stepsRemaining = 0;
			stepsDelayed = 0;
			waypointX[0] = x;
			waypointY[0] = y;
			this.x = waypointX[0] * 128 + boundaryDimension * 64;
			this.y = waypointY[0] * 128 + boundaryDimension * 64;
		}

		public void updateHitData(int type, int damage, int currentTime)
		{
			for(int hit = 0; hit < 4; hit++)
				if(hitsLoopCycle[hit] <= currentTime)
				{
					hitArray[hit] = damage;
					hitMarkTypes[hit] = type;
					hitsLoopCycle[hit] = currentTime + 70;
					return;
				}
		}
	}
}
