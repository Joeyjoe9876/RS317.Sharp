using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	//TODO: Move this directly to GladNet3.
	public abstract class BaseServerRequestHandler<TSpecificPayloadType> : IPeerMessageHandler<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>>
		where TSpecificPayloadType : BaseGameClientPayload
	{
		protected ILog Logger { get; }

		/// <inheritdoc />
		protected BaseServerRequestHandler([NotNull] ILog logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public abstract Task HandleMessage(IPeerSessionMessageContext<BaseGameServerPayload> context, TSpecificPayloadType payload);

		/// <inheritdoc />
		public virtual bool CanHandle(NetworkIncomingMessage<BaseGameClientPayload> message)
		{
			return message.Payload is TSpecificPayloadType;
		}

		/// <inheritdoc />
		public async Task<bool> TryHandleMessage(IPeerSessionMessageContext<BaseGameServerPayload> context, NetworkIncomingMessage<BaseGameClientPayload> message)
		{
			if(CanHandle(message))
			{
				await HandleMessage(context, message.Payload as TSpecificPayloadType)
					.ConfigureAwait(false);
				return true;
			}

			return false;
		}
	}
}