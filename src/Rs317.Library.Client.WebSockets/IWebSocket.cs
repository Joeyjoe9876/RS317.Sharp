using System;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public interface IWebSocket
	{
		event EventHandler<ArraySegment<byte>> OnDataReceived;

		WebSocketState State { get; }

		Task Send(byte[] bytes, int offset, int length);

		Task Close(WebSocketCloseCode code = WebSocketCloseCode.Normal, string reason = null);

		Task Receive();

		Task<bool> ConnectAsync(SocketCreationContext connectionInfo);
	}
}