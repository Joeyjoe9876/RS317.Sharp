using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using Rs317.Extended;
using Rs317.GladMMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class TitlescreenDependencyModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			//Register all required Authentication/Title modules.
			builder.RegisterModule<AuthenticationDependencyAutofacModule>();
			builder.RegisterModule(new CommonGameDependencyModule(GameSceneType.TitleScreen));
			builder.RegisterModule<TitleScreenUIDependenciesModule>();
			builder.RegisterModule(new EngineInterfaceRegisterationModule((int)GameSceneType.TitleScreen, typeof(GladMMOOpenTkClient).Assembly));

			builder.RegisterInstance(new ClientConfiguration(0, 0, true))
				.AsSelf();

			builder.RegisterInstance(new ConsoleLogger(LogLevel.All))
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<OpenTKRsGraphicsContext>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterType<OpenTkImageProducerFactory>()
				.AsSelf()
				.AsImplementedInterfaces()
				.SingleInstance();

			builder.RegisterType<ExtendedBufferFactory>()
				.AsSelf()
				.AsImplementedInterfaces()
				.SingleInstance();

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
		}
	}
}
