using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	[GameClientPayload(RsNetworkOperationCode.SetPlayerRightClickOptions)]
	public sealed class ServerSetPlayerRightClickOptionsPayload : BaseGameClientPayload
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
		public byte TopActionId { get; private set; }

		/// <summary>
		/// The text of the action.
		/// </summary>
		[WireMember(3)]
		public string ActionText { get; private set; }

		public ServerSetPlayerRightClickOptionsPayload(byte actionId, byte topActionId, [NotNull] string actionText)
		{
			ActionId = actionId;
			TopActionId = topActionId;
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
