using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rs317.Extended
{
	/// <summary>
	/// Class that offers high-level metadata about the payload assembly.
	/// </summary>
	public static class GameServerMetadataMarker
	{
		private static readonly Lazy<IReadOnlyDictionary<RsServerNetworkOperationCode, Type>> _ServerPayloadTypesByOpcode;
		private static readonly Lazy<IReadOnlyDictionary<RsClientNetworkOperationCode, Type>> _ClientPayloadTypesByOpcode;

		/// <summary>
		/// Collection of all payload types.
		/// </summary>
		public static IReadOnlyCollection<Type> PayloadTypes { get; }


		/// <summary>
		/// Collection of all server payload types as a dictionary
		/// with the key as the operation code.
		/// </summary>
		public static IReadOnlyDictionary<RsServerNetworkOperationCode, Type> ServerPayloadTypesByOpcode => _ServerPayloadTypesByOpcode.Value;

		/// <summary>
		/// Collection of all client payload types as a dictionary
		/// with the key as the operation code.
		/// </summary>
		public static IReadOnlyDictionary<RsClientNetworkOperationCode, Type> ClientPayloadTypesByOpcode => _ClientPayloadTypesByOpcode.Value;

		static GameServerMetadataMarker()
		{
			PayloadTypes = typeof(InitialConnectionRequestPacket)
				.Assembly
				.GetExportedTypes()
				.Where(t => typeof(BaseGameServerPayload).IsAssignableFrom(t) || typeof(BaseGameServerPayload).IsAssignableFrom(t))
				.Distinct()
				.ToArray();

			_ServerPayloadTypesByOpcode = new Lazy<IReadOnlyDictionary<RsServerNetworkOperationCode, Type>>(() => PayloadTypes
				.Where(t => typeof(BaseGameServerPayload).IsAssignableFrom(t))
				.ToDictionary(type => (RsServerNetworkOperationCode)type.GetCustomAttribute<GameServerPayloadAttribute>().Index), true);

			_ClientPayloadTypesByOpcode = new Lazy<IReadOnlyDictionary<RsClientNetworkOperationCode, Type>>(() => PayloadTypes
				.Where(t => typeof(BaseGameClientPayload).IsAssignableFrom(t))
				.ToDictionary(type => (RsClientNetworkOperationCode)type.GetCustomAttribute<GameClientPayloadAttribute>().Index), true);
		}
	}
}
