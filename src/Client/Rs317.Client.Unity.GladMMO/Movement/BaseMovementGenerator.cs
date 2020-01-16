using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using UnityEngine;

namespace Rs317.Sharp
{
	public abstract class MoveGenerator : IMovementGenerator<IWorldObject>
	{
		public Vector3 CurrentPosition { get; protected set; }

		public bool isStarted { get; protected set; } = false;

		public bool isFinished { get; private set; } = false;

		protected abstract Vector3 Start(IWorldObject entity, long currentTime);

		protected MoveGenerator()
		{

		}

		protected MoveGenerator(Vector3 currentPosition)
		{
			CurrentPosition = currentPosition;
		}

		/// <inheritdoc />
		public virtual void Update(IWorldObject entity, long currentTime)
		{
			if(isFinished)
				return;

			if(!isStarted)
			{
				CurrentPosition = Start(entity, currentTime);
				isStarted = true;
			}

			//it's possible after starting that it has finally become finished.
			//I encountered this with clientside pathing from Runescape emulation lol.
			if(isFinished)
				return;

			CurrentPosition = InternalUpdate(entity, currentTime); //don't update if we called Start
		}

		/// <summary>
		/// Called on <see cref="Update"/>
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="currentTime"></param>
		protected abstract Vector3 InternalUpdate(IWorldObject entity, long currentTime);

		protected void StopGenerator()
		{
			isFinished = true;
		}
	}

	/// <summary>
	/// Base for movement generators that control client and serverside movement simulation.
	/// </summary>
	/// <typeparam name="TDataInputType">The data input type.</typeparam>
	public abstract class BaseMovementGenerator<TDataInputType> : MoveGenerator
		where TDataInputType : class, IMovementData
	{
		/// <summary>
		/// The movement data used by this generator.
		/// </summary>
		protected TDataInputType MovementData { get; }

		/// <inheritdoc />
		protected BaseMovementGenerator([NotNull] TDataInputType movementData)
			: base()
		{
			MovementData = movementData ?? throw new ArgumentNullException(nameof(movementData));
		}

		protected BaseMovementGenerator([NotNull] TDataInputType movementData, Vector3 initialPosition)
			: base(initialPosition)
		{
			MovementData = movementData ?? throw new ArgumentNullException(nameof(movementData));
		}
	}
}
