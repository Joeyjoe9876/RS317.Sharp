using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public class RsMousePositionChangeEventArgs : EventArgs
	{
		/// <summary>
		/// The X screen coordinate of the click.
		/// </summary>
		public int ScreenCoordX { get; }

		/// <summary>
		/// The Y screen coordinate of the click.
		/// </summary>
		public int ScreenCoordY { get; }

		public RsMousePositionChangeEventArgs(int screenCoordX, int screenCordY)
		{
			ScreenCoordX = screenCoordX;
			ScreenCoordY = screenCordY;
		}
	}
}
