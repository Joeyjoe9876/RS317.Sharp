using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameServerPayload(RsServerNetworkOperationCode.SetPlayerRightClickOptions)]
	public sealed class ServerSetPlayerRightClickOptionsPayload : BaseGameServerPayload
	{
		/*int actionId = inStream.getUnsignedByteC();
		int actionAtTop = inStream.getUnsignedByteA();
		String actionText = inStream.getString();*/

		/// <summary>
		/// The index of the action id.
		/// </summary>
		[WireMember(1)]
		public byte ActionId { get; private set; }

		/// <summary>
		/// I don't know.
		/// </summary>
		[WireMember(2)]
		public bool IsPinnedAction { get; private set; }

		/// <summary>
		/// The text of the action.
		/// </summary>
		[SendSize(SendSizeAttribute.SizeType.UShort)]
		[WireMember(3)]
		public string ActionText { get; private set; }

		public ServerSetPlayerRightClickOptionsPayload(byte actionId, bool actionIsPinned, [NotNull] string actionText)
		{
			ActionId = actionId;
			IsPinnedAction = actionIsPinned;
			ActionText = actionText ?? throw new ArgumentNullException(nameof(actionText));
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ServerSetPlayerRightClickOptionsPayload()
		{
			
		}
	}
}
