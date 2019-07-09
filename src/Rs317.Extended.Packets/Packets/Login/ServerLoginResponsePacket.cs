using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Sharp;

namespace Rs317.Extended
{
	//STILL not the game protocol sadly.
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
			ResponseCode = responseCode;
			PrivilegeType = privilegeType;
			this.isClientFlagged = isClientFlagged;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		public ServerLoginResponsePacket()
		{
			
		}
	}
}
