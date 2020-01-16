using System;
using System.Collections.Generic;
using System.Text;
using Rs317.Sharp;

namespace Rs317
{
	public static class ChatModeTypeExtensions
	{
		/// <summary>
		/// Indicates if the mode is enabled such as:
		/// On or Friends Only.
		/// </summary>
		/// <param name="mode">The mode to check.</param>
		/// <returns></returns>
		public static bool isEnabled(this ChatModeType mode)
		{
			return mode < ChatModeType.Off;
		}
	}
}
