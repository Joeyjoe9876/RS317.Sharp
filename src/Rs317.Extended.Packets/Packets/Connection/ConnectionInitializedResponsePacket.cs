using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[Obsolete("Extended no longer uses the connection init or auth packets from Rs317")]
	[WireDataContract]
	public sealed class ConnectionInitializedResponsePacket
	{
		/// <summary>
		/// AKA the serverSessionKey.
		/// Used to initialize the packet header encryption.
		/// </summary>
		[WireMember(1)]
		public long ServerEncryptionInitializationVector { get; private set; }

		public ConnectionInitializedResponsePacket(long serverEncryptionInitializationVector)
		{
			ServerEncryptionInitializationVector = serverEncryptionInitializationVector;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ConnectionInitializedResponsePacket()
		{
			
		}
	}
}
