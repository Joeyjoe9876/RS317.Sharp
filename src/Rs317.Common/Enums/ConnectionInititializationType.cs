using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/// <summary>
	/// Enumeration of the types of connection initialization
	/// that the RS client can have.
	/// </summary>
	public enum ConnectionInititializationType : byte
	{
		/// <summary>
		/// The connection is being initialized with
		/// a login/auth request.
		/// </summary>
		LoginRequest = 14,

		Update = 15,

		NewConnection = 16,

		ReconnectionL = 18
	}
}
