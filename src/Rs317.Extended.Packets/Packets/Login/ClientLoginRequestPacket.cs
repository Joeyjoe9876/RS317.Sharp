using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Sharp;

namespace Rs317.Extended
{
	//Still not the game protocol.
	[WireDataContract]
	public sealed class ClientLoginRequestPacket
	{
		[WireMember(1)]
		public ClientLoginType LoginType { get; private set; }

		[WireMember(2)]
		private byte Size { get; set; }

		[WireMember(3)]
		private byte Unk1 { get; set; } = 255; //always 255?
		
		[WireMember(4)]
		public short ClientVersionIdentifier { get; private set; } = 317;

		/// <summary>
		/// Indicates if the client is a low memory client.
		/// </summary>
		[WireMember(5)]
		public bool isLowMemory { get; private set; }

		[KnownSize(9)]
		[WireMember(6)]
		private int[] CRCHashes { get; set; } = new int[9];

		[WireMember(7)]
		private byte Unk2 { get; set; } = 10;

		[WireMember(8)]
		public long ClientEncryptionInitializationVector { get; private set; }

		/// <summary>
		/// Same value as in: <see cref="ConnectionInitializedResponsePacket"/>
		/// </summary>
		[WireMember(9)]
		public long ServerEncryptionInitializationVector { get; private set; }

		/// <summary>
		/// The trackable client generated UID.
		/// </summary>
		[WireMember(10)]
		public int TrackingUID { get; private set; }

		[WireMember(11)]
		public string Username { get; private set; }

		[WireMember(12)]
		public string Password { get; private set; }

		public ClientLoginRequestPacket(ClientLoginType loginType, bool isLowMemory, 
			long clientEncryptionInitializationVector, long serverEncryptionInitializationVector, 
			int trackingUid, string username, string password)
		{
			LoginType = loginType;
			this.isLowMemory = isLowMemory;
			ClientEncryptionInitializationVector = clientEncryptionInitializationVector;
			ServerEncryptionInitializationVector = serverEncryptionInitializationVector;
			TrackingUID = trackingUid;
			Username = username;
			Password = password;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private ClientLoginRequestPacket()
		{
			
		}
	}
}
