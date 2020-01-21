using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class EditorWebGLSocketFactory : IRsSocketFactory
	{
		public IRsSocket Create(SocketCreationContext context)
		{
			EditorWebGLWebSocket editorWebSocketClient = new EditorWebGLWebSocket($"ws://{context.Endpoint}:{context.Port}");

			return new WebGLRsSocketClientAdapter(editorWebSocketClient);
		}
	}
}
