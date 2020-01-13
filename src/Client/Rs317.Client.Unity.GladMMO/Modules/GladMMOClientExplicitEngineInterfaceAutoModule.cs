using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Glader.Essentials;
using GladMMO;

namespace Rs317.GladMMO
{
	public sealed class GladMMOClientExplicitEngineInterfaceAutoModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EntityDataChangeTrackerTickable>()
				.As<IGameTickable>()
				.SingleInstance();

			builder.RegisterType<OnInitConnectNetworkClientInitializable>()
				.As<IGameInitializable>()
				.SingleInstance();

			/*[AdditionalRegisterationAs(typeof(INetworkConnectionEstablishedEventSubscribable))]
			[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
			public sealed class OnStartRestartNetworkClientHandlingInititablize : IGameStartable, INetworkConnectionEstablishedEventSubscribable*/
			builder.RegisterType<OnStartRestartNetworkClientHandlingInititablize>()
				.As<INetworkConnectionEstablishedEventSubscribable>()
				.As<IGameStartable>()
				.SingleInstance();
		}
	}
}
