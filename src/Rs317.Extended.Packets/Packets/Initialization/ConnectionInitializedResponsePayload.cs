using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameClientPayload(RsNetworkOperationCode.ConnectionInitialized)]
	public sealed class ConnectionInitializedResponsePayload : BaseGameClientPayload
	{
		/// <summary>
		/// AKA the serverSessionKey.
		/// Used to initialize the packet header encryption.
		/// </summary>
		[WireMember(1)]
		public long ServerEncryptionInitializationVector { get; private set; }

		public ConnectionInitializedResponsePayload(long serverEncryptionInitializationVector)
		{
			ServerEncryptionInitializationVector = serverEncryptionInitializationVector;
		}

		private ConnectionInitializedResponsePayload()
		{
			
		}
	}
}
