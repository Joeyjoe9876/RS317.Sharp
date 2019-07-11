using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Common.Logging;
using Common.Logging.Simple;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	public sealed class ServerApplicationBase : TcpServerServerApplicationBase<BaseGameServerPayload, BaseGameClientPayload>
	{
		private IManagedSessionFactory ManagedSessionFactory { get; }

		private IManagedClientSessionFactory ManagedClientSessionFactory { get; }

		/// <inheritdoc />
		public ServerApplicationBase([NotNull] NetworkAddressInfo serverAddress, [NotNull] ILog logger, [NotNull] IManagedSessionFactory managedSessionFactory, [NotNull] IManagedClientSessionFactory managedClientSessionFactory)
			: base(serverAddress, logger)
		{
			if(serverAddress == null) throw new ArgumentNullException(nameof(serverAddress));
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			ManagedSessionFactory = managedSessionFactory ?? throw new ArgumentNullException(nameof(managedSessionFactory));
			ManagedClientSessionFactory = managedClientSessionFactory ?? throw new ArgumentNullException(nameof(managedClientSessionFactory));

			if(Logger.IsDebugEnabled)
				Logger.Debug($"Created server base.");
		}

		/// <inheritdoc />
		public ServerApplicationBase([NotNull] NetworkAddressInfo serverAddress, [NotNull] INetworkMessageDispatchingStrategy<BaseGameServerPayload, BaseGameClientPayload> messageHandlingStrategy, [NotNull] IManagedSessionFactory managedSessionFactory, [NotNull] IManagedClientSessionFactory managedClientSessionFactory)
			: base(serverAddress, messageHandlingStrategy, new NoOpLogger())
		{
			if(serverAddress == null) throw new ArgumentNullException(nameof(serverAddress));
			if(messageHandlingStrategy == null) throw new ArgumentNullException(nameof(messageHandlingStrategy));
			ManagedSessionFactory = managedSessionFactory ?? throw new ArgumentNullException(nameof(managedSessionFactory));
			ManagedClientSessionFactory = managedClientSessionFactory ?? throw new ArgumentNullException(nameof(managedClientSessionFactory));

			if(Logger.IsDebugEnabled)
				Logger.Debug($"Created server base.");
		}

		/// <inheritdoc />
		protected override bool IsClientAcceptable(TcpClient tcpClient)
		{
			//TODO: This is where you would reject clients if you had a reason to.
			return true;
		}

		/// <inheritdoc />
		protected override IManagedNetworkServerClient<BaseGameServerPayload, BaseGameClientPayload> CreateIncomingSessionPipeline(TcpClient client)
		{
			return ManagedSessionFactory.Create(new ManagedSessionCreationContext(client));
		}

		protected override ManagedClientSession<BaseGameServerPayload, BaseGameClientPayload> CreateIncomingSession(IManagedNetworkServerClient<BaseGameServerPayload, BaseGameClientPayload> client, SessionDetails details)
		{
			return ManagedClientSessionFactory.Create(new ManagedClientSessionCreationContext(client, details));
		}
	}
}