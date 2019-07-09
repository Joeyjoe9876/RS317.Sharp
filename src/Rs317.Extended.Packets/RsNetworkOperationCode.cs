using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Extended
{
	/// <summary>
	/// Operation codes for the RS317 packets.
	/// </summary>
	public enum RsNetworkOperationCode : byte
	{
		/// <summary>
		/// Operation code for packet type that indicates a login
		/// success.
		/// </summary>
		LoginSuccessful = 2,
	}
}
