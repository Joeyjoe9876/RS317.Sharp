using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using GladNet;

namespace Rs317.Extended.Server
{
	class Program
	{
		static Task Main(string[] args)
		{
			ContainerBuilder builder = new ContainerBuilder();

			builder.RegisterModule<ApplicationAutofacModule>();
			builder.RegisterModule<SerializationModule>();
			builder.RegisterModule<SessionAutofacModule>();
			builder.RegisterModule<PayloadHandlerAutofacModule>();
			
			builder.RegisterInstance(new NetworkAddressInfo(IPAddress.Parse("127.0.0.1"), 43594));

			IContainer build = builder.Build();

			ServerApplicationBase applicationBase = build.Resolve<ServerApplicationBase>();

			applicationBase.StartServer();
			return applicationBase.BeginListening();
		}
	}
}
