using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class RsMouseInputEventArgs : RsMousePositionChangeEventArgs
	{
		/// <summary>
		/// Indicates if the click is a right-click.
		/// </summary>
		public bool IsRightClick { get; }

		public RsMouseInputEventArgs(int screenCoordX, int screenCordY, bool isRightClick) 
			: base(screenCoordX, screenCordY)
		{
			IsRightClick = isRightClick;
		}
	}
}
