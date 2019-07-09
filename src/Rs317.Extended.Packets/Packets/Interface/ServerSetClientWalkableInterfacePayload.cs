using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsNetworkOperationCode.SetClientWalkableInterface)]
	public sealed class ServerSetClientWalkableInterfacePayload : BaseGameServerPayload
	{
		/// <summary>
		/// The interface ID to set.
		/// </summary>
		[WireMember(1)]
		public short InterfaceId { get; private set; }

		public ServerSetClientWalkableInterfacePayload(short interfaceId)
		{
			InterfaceId = interfaceId;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetClientWalkableInterfacePayload()
		{
			
		}
	}
}
