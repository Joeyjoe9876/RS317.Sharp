using System;

namespace Rs317.Sharp
{
	public class WebSocketInvalidStateException : WebSocketException
	{
		public WebSocketInvalidStateException()
		{

		}

		public WebSocketInvalidStateException(string message) 
			: base(message)
		{

		}

		public WebSocketInvalidStateException(string message, Exception inner) 
			: base(message, inner)
		{

		}
	}
}