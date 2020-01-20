using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class DefaultRsSocketFactory : IRsSocketFactory
	{
		private IRunnableStarter RunnableStarter { get; }

		public DefaultRsSocketFactory(IRunnableStarter runnableStarter)
		{
			RunnableStarter = runnableStarter ?? throw new ArgumentNullException(nameof(runnableStarter));
		}

		public IRsSocket Create(SocketCreationContext context)
		{
			return new RSSocket(RunnableStarter, openSocket(context.Endpoint, context.Port));
		}

		public static TcpClient openSocket(string endpointAddress, int port)
		{
			if(port < 0) throw new ArgumentOutOfRangeException(nameof(port));
			if (string.IsNullOrEmpty(endpointAddress)) throw new ArgumentException("Value cannot be null or empty.", nameof(endpointAddress));

			//TODO: Should we check if it's connected??
			return new TcpClient(endpointAddress, port);
		}
	}
}
