using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using Rs317.GladMMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class InstanceServerDependencyModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterModule<GameServerNetworkClientAutofacModule>();
			builder.RegisterModule(new GameClientMessageHandlerAutofacModule(GameSceneType.InstanceServerScene, this.GetType().Assembly));
			builder.RegisterModule<GladMMONetworkSerializerAutofacModule>();
			builder.RegisterModule<GameplayDependencyRegisterationAutofacModule>();
			builder.RegisterModule<CharacterServiceDependencyAutofacModule>();

			builder.RegisterInstance<GameManager>(GladMMOProgram.RootGameManager)
				.As<IGameContextEventQueueable>()
				.As<IGameServiceable>()
				.AsSelf()
				.ExternallyOwned()
				.SingleInstance();

			builder.Register<GladMMOOpenTkClient>(context =>
				{
					//This is done to make sure only 1 is ever created.
					if(GladMMOProgram.RootClient == null)
					{
						GladMMOProgram.RootClient = new GladMMOOpenTkClient(context.Resolve<ClientConfiguration>(), context.Resolve<OpenTKRsGraphicsContext>(),
							context.Resolve<IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext>>(), context.Resolve<IBufferFactory>(),
							GladMMOProgram.RootGameManager);

						return GladMMOProgram.RootClient;
					}
					else
						return GladMMOProgram.RootClient;
				})
				.AsSelf()
				.AsImplementedInterfaces()
				.ExternallyOwned();

			//Register all required Authentication/Title modules.
			builder.RegisterModule(new CommonGameDependencyModule(GameSceneType.InstanceServerScene));
			builder.RegisterModule(new EngineInterfaceRegisterationModule((int)GameSceneType.InstanceServerScene, typeof(GladMMOOpenTkClient).Assembly));

			builder.RegisterInstance(new ConsoleLogger(LogLevel.All))
				.AsImplementedInterfaces()
				.SingleInstance();
		}
	}
}
