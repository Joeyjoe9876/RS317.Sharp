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
	}
}
