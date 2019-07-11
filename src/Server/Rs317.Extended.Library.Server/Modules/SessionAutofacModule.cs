using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace Rs317.Extended
{
	public sealed class SessionAutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DefaultManagedSessionFactory>()
				.As<IManagedSessionFactory>();

			builder.RegisterType<DefaultManagedClientSessionFactory>()
				.As<IManagedClientSessionFactory>();
		}
	}
}
