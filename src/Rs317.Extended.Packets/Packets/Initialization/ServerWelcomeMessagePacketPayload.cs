using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Extended;

namespace Rs317
{
	/*daysSinceRecoveryChange = inStream.getUnsignedByteC();
	unreadMessages = inStream.getUnsignedLEShortA();
	membership = inStream.getUnsignedByte();
	lastAddress = inStream.getMEBInt();
	daysSinceLogin = inStream.getUnsignedLEShort();*/

	[WireDataContract]
	[GameServerPayload(RsNetworkOperationCode.WelcomeMessage)]
	public sealed class ServerWelcomeMessagePacketPayload : BaseGameServerPayload
	{
		[WireMember(1)]
		public byte DaysSinceLastRecoveryChange { get; private set; }

		[WireMember(2)]
		public short UnreadMessageCount { get; private set; }

		[WireMember(3)]
		public int LastAddress { get; private set; }

		[WireMember(4)]
		public short DaysSinceLastLogin { get; private set; }

		public ServerWelcomeMessagePacketPayload(byte daysSinceLastRecoveryChange, short unreadMessageCount, int lastAddress, short daysSinceLastLogin)
		{
			DaysSinceLastRecoveryChange = daysSinceLastRecoveryChange;
			UnreadMessageCount = unreadMessageCount;
			LastAddress = lastAddress;
			DaysSinceLastLogin = daysSinceLastLogin;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerWelcomeMessagePacketPayload()
		{
			
		}
	}
}
