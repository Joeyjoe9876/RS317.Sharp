using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Sharp;

namespace Rs317.Extended
{
	//STILL not the game protocol sadly.
	[Obsolete("Extended no longer uses the connection init or auth packets from Rs317")]
	[WireDataContract]
	public sealed class ServerLoginResponsePacket
	{
		/// <summary>
		/// Yes, they send this code in the login response too.
		/// </summary>
		[WireMember(1)]
		public ConnectionInitializationResponseCode ResponseCode { get; private set; }

		/// <summary>
		/// Indicates if the response is a success response.
		/// </summary>
		public bool isSuccessful => ResponseCode == ConnectionInitializationResponseCode.SuccessfulLogin;

		//Only sent in login success.
		[Optional(nameof(isSuccessful))]
		[WireMember(2)]
		public ClientPrivilegeType PrivilegeType { get; private set; }

		//Only sent in login success.
		[Optional(nameof(isSuccessful))]
		[WireMember(3)]
		public bool isClientFlagged { get; private set; }

		public ServerLoginResponsePacket(ConnectionInitializationResponseCode responseCode, ClientPrivilegeType privilegeType, bool isClientFlagged)
		{
			if (!Enum.IsDefined(typeof(ConnectionInitializationResponseCode), responseCode)) throw new InvalidEnumArgumentException(nameof(responseCode), (int) responseCode, typeof(ConnectionInitializationResponseCode));
			if(!Enum.IsDefined(typeof(ClientPrivilegeType), privilegeType)) throw new InvalidEnumArgumentException(nameof(privilegeType), (int)privilegeType, typeof(ClientPrivilegeType));

			ResponseCode = responseCode;
			PrivilegeType = privilegeType;
			this.isClientFlagged = isClientFlagged;
		}

		/// <summary>
		/// Failure ctor.
		/// </summary>
		/// <param name="responseCode">The response code.</param>
		public ServerLoginResponsePacket(ConnectionInitializationResponseCode responseCode)
		{
			if(!Enum.IsDefined(typeof(ConnectionInitializationResponseCode), responseCode)) throw new InvalidEnumArgumentException(nameof(responseCode), (int)responseCode, typeof(ConnectionInitializationResponseCode));

			ResponseCode = responseCode;

			if(isSuccessful)
				throw new InvalidOperationException($"Cannot used failure CTOR for success.");
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		public ServerLoginResponsePacket()
		{
			
		}
	}
}
