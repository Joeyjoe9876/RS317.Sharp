using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public enum ClientPrivilegeType : byte
	{
		/// <summary>
		/// Regular ordinary players.
		/// </summary>
		Regular = 0,

		/// <summary>
		/// The player moderator privilege.
		/// </summary>
		PlayerModerator = 1,

		/// <summary>
		/// The JMOD privilege.
		/// </summary>
		Administrator = 2,

		SuperAdministrator = 3,
	}
}