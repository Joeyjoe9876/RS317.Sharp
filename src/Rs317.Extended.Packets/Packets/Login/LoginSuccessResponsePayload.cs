using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	[GameClientPayload(RsNetworkOperationCode.LoginSuccessful)]
	public sealed class LoginSuccessResponsePayload : BaseGameClientPayload
	{
		/// <summary>
		/// AKA the serverSessionKey.
		/// Used to initialize the packet header encryption.
		/// </summary>
		[WireMember(1)]
		public long ServerEncryptionInitializationVector { get; private set; }

		public LoginSuccessResponsePayload(long serverEncryptionInitializationVector)
		{
			ServerEncryptionInitializationVector = serverEncryptionInitializationVector;
		}

		private LoginSuccessResponsePayload()
		{
			
		}
	}
}
