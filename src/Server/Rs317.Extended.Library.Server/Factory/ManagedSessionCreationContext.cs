using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	public sealed class ManagedSessionCreationContext
	{
		public TcpClient Client { get; }

		/// <inheritdoc />
		public ManagedSessionCreationContext([NotNull] TcpClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}
