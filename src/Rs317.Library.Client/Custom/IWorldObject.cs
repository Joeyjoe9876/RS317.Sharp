using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IWorldObject
	{
		int CurrentX { get; }

		int CurrentY { get; }

		//Lowercase name perserved due to java.
		void setPos(int x, int y);

		void Teleport(int x, int y);

		void DirectSetPosition(int x, int y);

		void SetName(string name);

		void SetLevel(int level);

		/// <summary>
		/// Sets the last update tick as the current tick.
		/// </summary>
		void SetLastUpdateTick(int currentTick);
	}
}
