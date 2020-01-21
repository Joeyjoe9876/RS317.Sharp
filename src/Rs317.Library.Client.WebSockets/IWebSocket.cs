using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public interface IWebSocket
	{
		event WebSocketOpenEventHandler OnOpen;

		event WebSocketMessageEventHandler OnMessage;

		event WebSocketErrorEventHandler OnError;

		event WebSocketCloseEventHandler OnClose;

		WebSocketState State { get; }

		Task Send(byte[] bytes, int offset, int length);

		Task Close(WebSocketCloseCode code = WebSocketCloseCode.Normal, string reason = null);

		Task Receive();
	}
}