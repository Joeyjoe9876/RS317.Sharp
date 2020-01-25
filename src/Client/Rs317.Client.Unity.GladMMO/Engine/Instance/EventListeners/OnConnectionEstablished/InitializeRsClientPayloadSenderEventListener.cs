using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using GladNet;
using Rs317.GladMMO;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeRsClientPayloadSenderEventListener : BaseSingleEventListenerInitializable<INetworkConnectionEstablishedEventSubscribable>
	{
		private IPeerPayloadSendService<GameClientPacketPayload> SendService { get; }

		public InitializeRsClientPayloadSenderEventListener(INetworkConnectionEstablishedEventSubscribable subscriptionService,
			[NotNull] IPeerPayloadSendService<GameClientPacketPayload> sendService)
			: base(subscriptionService)
		{
			SendService = sendService ?? throw new ArgumentNullException(nameof(sendService));
		}

		protected override void OnEventFired(object source, EventArgs args)
		{
			GladMMOUnityClient.SendService = SendService;
		}
	}
}
