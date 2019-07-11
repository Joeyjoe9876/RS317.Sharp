using System;
using System.Collections.Generic;
using System.Text;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	public sealed class DefaultManagedSessionFactory : IManagedSessionFactory
	{
		private INetworkSerializationService Serializer { get; }

		private ILog Logger { get; }

		/// <inheritdoc />
		public DefaultManagedSessionFactory([NotNull] INetworkSerializationService serializer, [NotNull] ILog logger)
		{
			Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public IManagedNetworkServerClient<BaseGameServerPayload, BaseGameClientPayload> Create(ManagedSessionCreationContext context)
		{
			return new Extended317UnmanagedNetworkClient<DotNetTcpClientNetworkClient, BaseGameClientPayload, BaseGameServerPayload, IGamePacketPayload>(new DotNetTcpClientNetworkClient(context.Client), Serializer, Logger)
				.AsManagedSession();
		}
	}
}