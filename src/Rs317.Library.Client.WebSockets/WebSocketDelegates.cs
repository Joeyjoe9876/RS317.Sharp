using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public delegate void WebSocketOpenEventHandler();
	public delegate void WebSocketMessageEventHandler(byte[] data);
	public delegate void WebSocketErrorEventHandler(string errorMsg);
	public delegate void WebSocketCloseEventHandler(WebSocketCloseCode closeCode);
}
