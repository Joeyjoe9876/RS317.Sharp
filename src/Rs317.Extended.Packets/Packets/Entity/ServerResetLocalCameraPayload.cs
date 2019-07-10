using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsNetworkOperationCode.SetChatModeStatus)]
	public sealed class ServerResetLocalCameraPayload : BaseGameServerPayload
	{
		//No data in this packet, just a command.

		public ServerResetLocalCameraPayload()
		{
			
		}
	}
}
