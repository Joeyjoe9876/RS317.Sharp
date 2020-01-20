using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class SocketCreationContext
	{
		public string Endpoint { get; }

		public int Port { get; }

		public SocketCreationContext(string endpoint, int port)
		{
			if (port <= 0) throw new ArgumentOutOfRangeException(nameof(port));

			Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
			Port = port;
		}
	}
}
