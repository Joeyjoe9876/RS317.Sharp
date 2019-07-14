using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;
using Rs317.Sharp;

namespace Rs317.Extended
{
	[ServerOperationType]
	public sealed class SessionClaimRequestPayloadHandler : BaseServerRequestHandler<ClientSessionClaimRequestPayload>
	{
		public SessionClaimRequestPayloadHandler([NotNull] ILog logger) 
			: base(logger)
		{

		}

		public override async Task HandleMessage(IPeerSessionMessageContext<BaseGameServerPayload> context, ClientSessionClaimRequestPayload payload)
		{
			//params never null

			//TODO: Implement authentication with the payload's JWT.

			//If successful, we should then queue them up for creation on the simulation thread.
			
			if(Logger.IsDebugEnabled)
				Logger.Debug($"Recieved SessionClaim: {payload.JWT}");

			if(Logger.IsDebugEnabled)
				Logger.Debug($"Sending Welcome Packet.");

			//TODO: This is just demo code, not how it should be handled.

			//208
			await context.PayloadSendService.SendMessage(new ServerSetClientWalkableInterfacePayload(-1));

			//Right-click player options
			for (int i = 0; i < 3; i++)
				await context.PayloadSendService.SendMessage(new ServerSetPlayerRightClickOptionsPayload((byte) (i + 1), false, "Test"));

			//249
			await context.PayloadSendService.SendMessage(new ServerSetLocalPlayerNetworkStatusPayload(true, 1));

			//206
			await context.PayloadSendService.SendMessage(new ServerSetChatModeStatusPayload(ChatModeType.On, ChatModeType.On, ChatModeType.On));

			//107
			await context.PayloadSendService.SendMessage(new ServerResetLocalCameraPayload());

			//68
			await context.PayloadSendService.SendMessage(new ServerResetInterfaceButtonStatePayload());

			await SendTabInterfaces(context.PayloadSendService);

			//176
			await context.PayloadSendService.SendMessage(new ServerWelcomeMessagePacketPayload(5, 10, 5, 69));

			//73
			await context.PayloadSendService.SendMessage(new ServerSetClientRegionPayload(402, 402));

			//Stub
			await context.PayloadSendService.SendMessage(new ServerUpdateLocalPlayerPayload(new Vector2<short>(50, 50)));
		}

		private async Task SendTabInterfaces(IPeerPayloadSendService<BaseGameServerPayload> contextPayloadSendService)
		{
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(3971, 1));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(638, 2));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(3213, 3));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(1644, 4));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(5608, 5));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(1151, 6));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(short.MaxValue, 7)); //TODO: This one is wrong.
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(5065, 8));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(5715, 9));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(2449, 10));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(4445, 11));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(147, 12));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(962, 13));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(2423, 0));
			await contextPayloadSendService.SendMessage(new ServerLinkTabsToInterfacePayload(5855, 0));
		}
	}
}
