using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Extended
{
	/// <summary>
	/// Contract for types that contain or expose user details.
	/// </summary>
	public interface IUserAuthenticationDetailsContainer
	{
		/// <summary>
		/// Username for the authentication request.
		/// </summary>
		string UserName { get; }

		/// <summary>
		/// Password for the authentication request.
		/// </summary>
		string Password { get; }
	}
}