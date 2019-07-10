using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	/*int sidebarId = inStream.getUnsignedLEShort();
	 int interfaceId = inStream.getUnsignedByteA();*/

	//I can't even believe this is a payload lol.
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.LinkTabsToInterface)]
	public sealed class ServerLinkTabsToInterfacePayload : BaseGameServerPayload
	{
		[WireMember(1)]
		public short SidebarId { get; private set; }

		[WireMember(2)]
		public byte InterfaceId { get; private set; }

		public ServerLinkTabsToInterfacePayload(short sidebarId, byte interfaceId)
		{
			SidebarId = sidebarId;
			InterfaceId = interfaceId;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerLinkTabsToInterfacePayload()
		{
			
		}
	}
}
