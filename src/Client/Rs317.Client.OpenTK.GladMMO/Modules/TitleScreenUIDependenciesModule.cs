using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Features.AttributeFilters;
using Glader.Essentials;

namespace Rs317.GladMMO
{
	public sealed class TitleScreenUIDependenciesModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<LoginButtonClientAdapter>()
				.Keyed<IUIButton>(ClientUIDependencyKey.LoginButton)
				.SingleInstance();

			builder.RegisterType<PasswordFieldClientAdapter>()
				.Keyed<IUIText>(ClientUIDependencyKey.Password)
				.SingleInstance();

			builder.RegisterType<UsernameFieldClientAdapter>()
				.Keyed<IUIText>(ClientUIDependencyKey.UserName)
				.SingleInstance();
		}
	}
}
