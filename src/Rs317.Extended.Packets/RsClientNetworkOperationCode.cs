using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Extended
{
	/// <summary>
	/// Client operation codes for the RS317 packets.
	/// </summary>
	public enum RsClientNetworkOperationCode : byte
	{
		//TODO: make sure this opcode isn't in use. It's custom.
		SessionClaimRequest = 255,
	}
}
