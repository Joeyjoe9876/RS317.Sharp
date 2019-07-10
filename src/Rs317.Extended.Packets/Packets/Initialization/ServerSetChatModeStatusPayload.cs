using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Sharp;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.SetChatModeStatus)]
	public sealed class ServerSetChatModeStatusPayload : BaseGameServerPayload
	{
		[WireMember(1)]
		public ChatModeType PublicChatMode { get; private set; }

		[WireMember(2)]
		public ChatModeType PrivateChatMode { get; private set; }

		[WireMember(4)]
		public ChatModeType TradeChatMode { get; private set; }

		public ServerSetChatModeStatusPayload(ChatModeType publicChatMode, ChatModeType privateChatMode, ChatModeType tradeChatMode)
		{
			if (!Enum.IsDefined(typeof(ChatModeType), publicChatMode)) throw new InvalidEnumArgumentException(nameof(publicChatMode), (int) publicChatMode, typeof(ChatModeType));
			if (!Enum.IsDefined(typeof(ChatModeType), privateChatMode)) throw new InvalidEnumArgumentException(nameof(privateChatMode), (int) privateChatMode, typeof(ChatModeType));
			if(!Enum.IsDefined(typeof(ChatModeType), tradeChatMode)) throw new InvalidEnumArgumentException(nameof(tradeChatMode), (int)tradeChatMode, typeof(ChatModeType));

			PublicChatMode = publicChatMode;
			PrivateChatMode = privateChatMode;
			TradeChatMode = tradeChatMode;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetChatModeStatusPayload()
		{
			
		}
	}
}
