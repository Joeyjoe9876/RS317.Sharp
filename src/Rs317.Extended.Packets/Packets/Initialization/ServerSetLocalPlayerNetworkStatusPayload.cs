using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameClientPayload(RsNetworkOperationCode.SetPlayerNetworkStatus)]
	public sealed class ServerSetLocalPlayerNetworkStatusPayload : BaseGameServerPayload
	{
		/// <summary>
		/// Indicates the membership status of the player.
		/// </summary>
		[WireMember(1)]
		public bool isMember { get; private set; }

		/// <summary>
		/// AKA the PID.
		/// </summary>
		[WireMember(2)]
		public short PlayerID { get; private set; }

		public ServerSetLocalPlayerNetworkStatusPayload(bool isMember, short playerId)
		{
			this.isMember = isMember;
			PlayerID = playerId;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetLocalPlayerNetworkStatusPayload()
		{
			
		}
	}
}
