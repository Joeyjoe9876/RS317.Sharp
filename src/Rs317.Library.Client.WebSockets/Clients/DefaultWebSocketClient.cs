using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class DefaultWebSocketClient : IWebSocket
	{
		public event WebSocketOpenEventHandler OnOpen;

		public event WebSocketMessageEventHandler OnMessage;

		public event WebSocketErrorEventHandler OnError;

		public event WebSocketCloseEventHandler OnClose;

		private Uri EndpointUri { get; }

		private ClientWebSocket m_Socket { get; set; } = new ClientWebSocket();

		private byte[] InternalBuffer { get; } = new byte[8192];

		private bool isReading = false;

		private readonly object SyncObj = new object();

		public DefaultWebSocketClient(string url)
		{
			EndpointUri = new Uri(url);

			string protocol = EndpointUri.Scheme;
			if(!protocol.Equals("ws") && !protocol.Equals("wss"))
				throw new ArgumentException("Unsupported protocol: " + protocol);
		}

		public async Task Connect()
		{
			try
			{
				m_Socket = new ClientWebSocket();

				await m_Socket.ConnectAsync(EndpointUri, CancellationToken.None);
				OnOpen?.Invoke();
			}
			catch(Exception ex)
			{
				OnError?.Invoke(ex.Message);
				OnClose?.Invoke(WebSocketCloseCode.Abnormal);
			}
		}

		public WebSocketState State
		{
			get
			{
				switch(m_Socket.State)
				{
					case System.Net.WebSockets.WebSocketState.Connecting:
						return WebSocketState.Connecting;

					case System.Net.WebSockets.WebSocketState.Open:
						return WebSocketState.Open;

					case System.Net.WebSockets.WebSocketState.CloseSent:
					case System.Net.WebSockets.WebSocketState.CloseReceived:
						return WebSocketState.Closing;

					case System.Net.WebSockets.WebSocketState.Closed:
						return WebSocketState.Closed;

					default:
						return WebSocketState.Closed;
				}
			}
		}

		public async Task Send(byte[] bytes, int offset, int length)
		{
			try
			{
				await m_Socket.SendAsync(new ArraySegment<byte>(bytes, offset, length), WebSocketMessageType.Binary, true, CancellationToken.None);
			}
			catch(Exception e)
			{
				Console.WriteLine($"{nameof(DefaultWebSocketClient)} failed to send message. Reason: {e}");
				throw;
			}
		}

		public async Task Receive()
		{
			lock (SyncObj)
			{
				//Websocket only allows one receive call at a time.
				if(isReading)
					return;

				isReading = true;
			}

			try
			{
				WebSocketReceiveResult result = await m_Socket.ReceiveAsync(new ArraySegment<byte>(InternalBuffer), CancellationToken.None);

				if (result == null)
					return;

				if (result.MessageType == WebSocketMessageType.Binary)
				{
					lock (SyncObj)
					{
						OnMessage?.Invoke(new ArraySegment<byte>(InternalBuffer, 0, result.Count));
					}
				}
				else if (result.MessageType == WebSocketMessageType.Close)
				{
					await Close();
					OnClose?.Invoke(WebSocketHelpers.ParseCloseCodeEnum((int) result.CloseStatus));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"{nameof(DefaultWebSocketClient)} failed to receive message. Reason: {e}");
				throw;
			}
			finally
			{
				isReading = false;
			}
		}

		//TODO: Handle code better.
		public async Task Close(WebSocketCloseCode code = WebSocketCloseCode.Normal, string reason = null)
		{
			if(State == WebSocketState.Open)
			{
				await m_Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
			}
		}
	}
}