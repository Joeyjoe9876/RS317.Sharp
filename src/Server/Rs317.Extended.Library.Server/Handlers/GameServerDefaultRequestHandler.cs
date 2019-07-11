using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;

namespace Rs317.Extended
{
	/// <summary>
	/// This is the default handler that is invoked when an unknown payload is encountered.
	/// Or a payload is encountered that doesn't have a registered handler.
	/// </summary>
	public sealed class GameServerDefaultRequestHandler : IPeerPayloadSpecificMessageHandler<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>>
	{
		private ILog Logger { get; }

		/// <inheritdoc />
		public GameServerDefaultRequestHandler(ILog logger)
		{
			if(logger == null) throw new ArgumentNullException(nameof(logger));

			Logger = logger;
		}

		/// <inheritdoc />
		public Task HandleMessage(IPeerSessionMessageContext<BaseGameServerPayload> context, BaseGameClientPayload payload)
		{
			if(Logger.IsWarnEnabled)
				Logger.Warn($"Recieved unhandable Payload: {payload.GetType().Name} ConnectionId: {context.Details.ConnectionId}");

			return Task.CompletedTask;
		}
	}
}