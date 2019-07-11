using System;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	/// <summary>
	/// Client payload sent by a client in an attempt to
	/// claim a an open session on the server.
	/// </summary>
	[WireDataContract]
	[GameClientPayload(RsClientNetworkOperationCode.SessionClaimRequest)]
	public sealed class ClientSessionClaimRequestPayload : BaseGameClientPayload
	{
		//TODO: This is not save to sent over the wire in plaintext so it should be encrypted somehow.
		/// <summary>
		/// The JWT string that can be used to authorize
		/// the user.
		/// </summary>
		[WireMember(1)]
		public string JWT { get; }

		/// <inheritdoc />
		public ClientSessionClaimRequestPayload(string jwt)
		{
			if(string.IsNullOrWhiteSpace(jwt)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(jwt));
			if(characterId < 0) throw new ArgumentOutOfRangeException(nameof(characterId));

			JWT = jwt;
		}

		//Serializer ctor
		private ClientSessionClaimRequestPayload()
		{

		}
	}
}