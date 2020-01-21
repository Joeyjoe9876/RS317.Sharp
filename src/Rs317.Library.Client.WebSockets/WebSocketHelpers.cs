using System;
using System.Collections.Generic;
using System.Linq;

namespace Rs317.Sharp
{
	public static class WebSocketHelpers
	{
		public static HashSet<int> KnownCloseCodes { get; } = new HashSet<int>(((WebSocketCloseCode[])Enum.GetValues(typeof(WebSocketCloseCode))).Select(v => (int)v));

		public static WebSocketCloseCode ParseCloseCodeEnum(int closeCode)
		{
			if(KnownCloseCodes.Contains(closeCode))
				return (WebSocketCloseCode)closeCode;

			return WebSocketCloseCode.Undefined;
		}

		public static WebSocketException GetErrorMessageFromCode(int errorCode, Exception inner)
		{
			switch(errorCode)
			{
				case -1: return new WebSocketUnexpectedException("WebSocket instance not found.", inner);
				case -2: return new WebSocketInvalidStateException("WebSocket is already connected or in connecting state.", inner);
				case -3: return new WebSocketInvalidStateException("WebSocket is not connected.", inner);
				case -4: return new WebSocketInvalidStateException("WebSocket is already closing.", inner);
				case -5: return new WebSocketInvalidStateException("WebSocket is already closed.", inner);
				case -6: return new WebSocketInvalidStateException("WebSocket is not in open state.", inner);
				case -7: return new WebSocketInvalidArgumentException("Cannot close WebSocket. An invalid code was specified or reason is too long.", inner);
				default: return new WebSocketUnexpectedException("Unknown error.", inner);
			}
		}
	}
}