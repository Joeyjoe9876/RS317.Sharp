using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Glader.Essentials;
using GladMMO;
using GladNet;

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

			builder.RegisterType<ClientEntityDespawnTickable>()
				.As<IGameTickable>()
				.As<IEntityDeconstructionStartingEventSubscribable>()
				.As<IEntityDeconstructionFinishedEventSubscribable>()
				.As<IGameInitializable>()
				.SingleInstance();

			builder.RegisterType<EntitySpawnTickable>()
				.As<IEntityCreationStartingEventSubscribable>()
				.As<IEntityCreationFinishedEventSubscribable>()
				.As<IGameTickable>()
				.As<IGameInitializable>()
				.SingleInstance();

			builder.RegisterType<NetworkVisibilityChangeEventHandler>()
				.As<INetworkEntityVisibilityLostEventSubscribable>()
				.As<IPeerMessageHandler<GameServerPacketPayload, GameClientPacketPayload>>()
				.SingleInstance();
		}
	}
}
