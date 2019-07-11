using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.SetRegion)]
	public sealed class ServerSetClientRegionPayload : BaseGameServerPayload
	{
		[WireMember(1)]
		public short RegionX { get; private set; }

		[WireMember(2)]
		public short RegionY { get; private set; }

		public ServerSetClientRegionPayload(short regionX, short regionY)
		{
			RegionX = regionX;
			RegionY = regionY;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetClientRegionPayload()
		{
			
		}
	}
}
