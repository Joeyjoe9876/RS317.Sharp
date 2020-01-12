using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public enum TitleScreenState
	{
		/// <summary>
		/// Default titlescreen state.
		/// While NewUser and Login button are visible.
		/// </summary>
		Default = 0,

		/// <summary>
		/// State where the UI is in the Login section.
		/// </summary>
		LoginBox = 2,

		/// <summary>
		/// State where the user is in the new user box.
		/// </summary>
		NewUserBox = 3,
	}
}
