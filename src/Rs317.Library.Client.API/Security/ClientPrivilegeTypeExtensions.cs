using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public static class ClientPrivilegeTypeExtensions
	{
		/// <summary>
		/// Indicates if the client privilege type
		/// is elevated beyond a regular player.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool isPrivilegeElevated(this ClientPrivilegeType type)
		{
			return type >= ClientPrivilegeType.PlayerModerator;
		}
	}
}
