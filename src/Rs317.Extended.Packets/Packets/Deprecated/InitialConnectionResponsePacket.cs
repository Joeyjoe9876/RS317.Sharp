using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FreecraftCore.Serializer;
using JetBrains.Annotations;
using Rs317.Sharp;

namespace Rs317.Extended
{
	/// <summary>
	/// The response packet DTO for the login initialization.
	/// This is doesn't match the game protocol, like most logins in other games.
	/// </summary>
	[Obsolete("Extended no longer uses the connection init or auth packets from Rs317")]
	[WireDataContract]
	public sealed class InitialConnectionResponsePacket
	{
		[KnownSize(8)]
		[WireMember(1)]
		public byte[] MagicNumber { get; private set; }

		/// <summary>
		/// The response code for the connection initialization request.
		/// </summary>
		[WireMember(2)]
		public ConnectionInitializationResponseCode ResponseCode { get; }

		public InitialConnectionResponsePacket([NotNull] byte[] magicNumber, ConnectionInitializationResponseCode responseCode)
		{
			if (!Enum.IsDefined(typeof(ConnectionInitializationResponseCode), responseCode)) throw new InvalidEnumArgumentException(nameof(responseCode), (int) responseCode, typeof(ConnectionInitializationResponseCode));
			MagicNumber = magicNumber ?? throw new ArgumentNullException(nameof(magicNumber));
			ResponseCode = responseCode;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private InitialConnectionResponsePacket()
		{
			
		}
	}
}
