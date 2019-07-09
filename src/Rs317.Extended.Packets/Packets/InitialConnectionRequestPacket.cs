using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;
using Rs317.Sharp;

namespace Rs317.Extended
{

	/*stream.position = 0;
	stream.put(14);
	stream.put(nameHash);
	socket.write(2, stream.buffer);*/

	/// <summary>
	/// Serializable structure that represents the
	/// initial connection packet sent by the client.
	/// </summary>
	[WireDataContract]
	public sealed class InitialConnectionRequestPacket //no base type, since it's unique.
	{
		/// <summary>
		/// First byte sent inidicating the type of the connection.
		/// </summary>
		[WireMember(1)]
		public ConnectionInititializationType ConnectionType { get; private set; }

		[WireMember(2)]
		public byte NameHash { get; private set; }

		public InitialConnectionRequestPacket(ConnectionInititializationType connectionType, byte nameHash)
		{
			ConnectionType = connectionType;
			NameHash = nameHash;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private  InitialConnectionRequestPacket()
		{
			
		}
	}
}
