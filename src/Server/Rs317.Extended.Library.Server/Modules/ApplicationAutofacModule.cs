using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Common.Logging;
using Common.Logging.Simple;

namespace Rs317.Extended
{
	public sealed class ApplicationAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ServerApplicationBase>()
				.AsSelf();

			builder.Register(context => LogLevel.Debug)
				.As<LogLevel>();

			builder.RegisterType<ConsoleLogger>()
				.As<ILog>()
				.AsSelf()
				.SingleInstance();
		}
	}
}
