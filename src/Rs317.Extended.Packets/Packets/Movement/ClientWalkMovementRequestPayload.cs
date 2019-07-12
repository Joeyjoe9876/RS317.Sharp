using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameClientPayload(RsClientNetworkOperationCode.LeftClickWalkRequest)]
	public sealed class ClientWalkMovementRequestPayload : BaseGameClientPayload
	{
		[WireMember(1)]
		[SendSize(SendSizeAttribute.SizeType.Byte)]
		public Vector2<short>[] PathPoints { get; private set; }

		[WireMember(2)]
		public bool isRunningRequested { get; private set; }

		public ClientWalkMovementRequestPayload([NotNull] Vector2<short>[] pathPoints, bool isRunningRequested)
		{
			PathPoints = pathPoints ?? throw new ArgumentNullException(nameof(pathPoints));
			this.isRunningRequested = isRunningRequested;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ClientWalkMovementRequestPayload(bool isRunningRequested)
		{
			this.isRunningRequested = isRunningRequested;
		}
	}
}
