using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	/// <summary>
	/// client session.
	/// </summary>
	public sealed class GameClientSession : ManagedClientSession<BaseGameServerPayload, BaseGameClientPayload>
	{
		/// <summary>
		/// The message handlers service.
		/// </summary>
		private MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>> MessageHandlers { get; }

		//TODO: One day this design fault will be fixed.
		public static MockedPayloadInterceptorService MockedInterceptorService { get; } = new MockedPayloadInterceptorService();

		private ILog Logger { get; }

		/// <inheritdoc />
		public GameClientSession(IManagedNetworkServerClient<BaseGameServerPayload, BaseGameClientPayload> internalManagedNetworkClient,
			SessionDetails details,
			MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>> messageHandlers,
			[NotNull] ILog logger)
			: base(internalManagedNetworkClient, details)
		{
			MessageHandlers = messageHandlers ?? throw new ArgumentNullException(nameof(messageHandlers));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public override Task OnNetworkMessageRecieved(NetworkIncomingMessage<BaseGameClientPayload> message)
		{
			Console.WriteLine($"Recieved: {message.Payload.GetType().Name}");
			return MessageHandlers.TryHandleMessage(new DefaultSessionMessageContext<BaseGameServerPayload>(this.Connection, this.SendService, MockedInterceptorService, Details), message);
		}
	}
}