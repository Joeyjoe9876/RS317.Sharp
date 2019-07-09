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
		/// Operation code of the packet that will set the an interface
		/// that is walkable.
		/// </summary>
		SetClientWalkableInterface = 208,

		SetPlayerRightClickOptions = 104,
	}
}
