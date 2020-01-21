using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class DefaultWebSocketClientFactory : IRsSocketFactory
	{
		public IRsSocket Create(SocketCreationContext context)
		{
			DefaultWebSocketClient editorWebSocketClient = new DefaultWebSocketClient($"ws://{context.Endpoint}:{context.Port}");
			editorWebSocketClient.Connect();

			return new WebGLRsSocketClientAdapter(editorWebSocketClient);
		}
	}
}
