using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IGameStateHookable
	{
		/// <summary>
		/// Hookable variable for when the logged
		/// in state of the client changes.
		/// </summary>
		HookableVariable<bool> LoggedIn { get; }
	}
}
