using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Common.Logging;
using GladNet;

namespace Rs317.Extended
{
	public sealed class PayloadHandlerAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<GameServerDefaultRequestHandler>()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>>>()
				.As<MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>>>()
				.SingleInstance();
		}
	}
}
