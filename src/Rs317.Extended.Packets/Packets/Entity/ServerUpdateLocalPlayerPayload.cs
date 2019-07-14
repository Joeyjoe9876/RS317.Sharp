using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.LocalPlayerUpdate)]
	public sealed class ServerUpdateLocalPlayerPayload : BaseGameServerPayload
	{
		[WireMember(1)]
		public Vector2<short> Position { get; set; }

		public ServerUpdateLocalPlayerPayload([NotNull] Vector2<short> position)
		{
			Position = position ?? throw new ArgumentNullException(nameof(position));
		}

		private ServerUpdateLocalPlayerPayload()
		{
			
		}
	}
}
