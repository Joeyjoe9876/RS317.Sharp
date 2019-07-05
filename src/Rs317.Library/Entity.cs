
using System;

public class Entity : Animable
{
	public int entScreenX;

	public int entScreenY;

	public int index = -1;

	public int[] waypointX;

	public int[] waypointY;
	public int interactingEntity;
	int stepsDelayed;

	int degreesToTurn;

	protected int runAnimationId { get; set; }

	public String overheadTextMessage;
	public int height;
	public int turnDirection;
	public int standAnimationId { get; protected set; }
	protected int standTurnAnimationId { get; set; }
	int chatColour;
	int[] hitArray;
	int[] hitMarkTypes;
	int[] hitsLoopCycle;
	public int queuedAnimationId { get; private set; }
	protected int queuedAnimationFrame { get; private set; }
	public int queuedAnimationDuration;
	protected int graphicId { get; private set; }
	protected int currentAnimationId { get; private set; }
	int currentAnimationTimeRemaining;
	int graphicEndCycle;
	protected int graphicHeight { get; private set; }
	int waypointCount;
	public int animation;
	protected int currentAnimationFrame { get; private set; }
	int currentAnimationDuration;
	protected int animationDelay { get; private set; }
	int currentAnimationLoopCount;
	int chatEffect;
	public int loopCycleStatus;
	public int currentHealth;
	public int maxHealth;
	public int textCycle { get; set; }
	int lastUpdateTick;
	int faceTowardX;
	int faceTowardY;
	int boundaryDimension;
	public bool dynamic { get; set; }
	int stepsRemaining;
	int startX;
	int endX;
	int startY;
	int endY;
	int tickStart;
	int tickEnd;
	int direction;
	public int x;
	public int y;
	int currentRotation;
	bool[] waypointRan;
	protected int walkAnimationId { get; set; }
	protected int turnAboutAnimationId { get; set; }
	protected int turnRightAnimationId { get; set; }
	protected int turnLeftAnimationId { get; set; }

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
