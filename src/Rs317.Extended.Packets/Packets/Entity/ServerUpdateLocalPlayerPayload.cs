using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.LocalPlayerUpdate)]
	public sealed class ServerUpdateLocalPlayerPayload : BaseGameServerPayload
	{
		//TODO: Implement this. This is just a stub for now.

		public ServerUpdateLocalPlayerPayload()
		{
			
		}
	}
}
