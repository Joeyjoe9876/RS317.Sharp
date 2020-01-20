using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class WebGLTcpClientRsSocketFactory : IRsSocketFactory
	{
		public IRsSocket Create(SocketCreationContext context)
		{
			return new WebGLTcpClientRsSocket(new TcpClient(context.Endpoint, context.Port));
		}
	}
}
