using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class RsKeyEventArgs : EventArgs
	{
		/// <summary>
		/// The pre-translated RS input key code.
		/// (not the platform specific keycode value).
		/// </summary>
		public int RsKeyCode { get; }

		public char KeyChar { get; }

		public RsKeyEventArgs(int rsKeyCode, char keyChar)
		{
			if (rsKeyCode < 0) throw new ArgumentOutOfRangeException(nameof(rsKeyCode));

			RsKeyCode = rsKeyCode;
			KeyChar = keyChar;
		}
	}
}
