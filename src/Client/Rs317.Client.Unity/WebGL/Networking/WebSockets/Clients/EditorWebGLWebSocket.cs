using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class EditorWebGLWebSocket : IWebSocket
	{
		public event WebSocketOpenEventHandler OnOpen;

		public event WebSocketMessageEventHandler OnMessage;

		public event WebSocketErrorEventHandler OnError;

		public event WebSocketCloseEventHandler OnClose;

		private Uri uri;
		private ClientWebSocket m_Socket = new ClientWebSocket();

		private readonly object Lock = new object();

		private bool isSending = false;
		private List<ArraySegment<byte>> sendBytesQueue = new List<ArraySegment<byte>>();
		private List<ArraySegment<byte>> sendTextQueue = new List<ArraySegment<byte>>();

		public EditorWebGLWebSocket(string url)
		{
			uri = new Uri(url);

			string protocol = uri.Scheme;
			if(!protocol.Equals("ws") && !protocol.Equals("wss"))
				throw new ArgumentException("Unsupported protocol: " + protocol);
		}

		public async Task Connect()
		{
			try
			{
				m_Socket = new ClientWebSocket();

				await m_Socket.ConnectAsync(uri, CancellationToken.None);
				OnOpen?.Invoke();

				await Receive();
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

		public Task Send(byte[] bytes, int offset, int length)
		{
			// return m_Socket.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None);
			return SendMessage(sendBytesQueue, WebSocketMessageType.Binary, new ArraySegment<byte>(bytes, offset, length));
		}

		public Task SendText(string message)
		{
			var encoded = Encoding.UTF8.GetBytes(message);

			// m_Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
			return SendMessage(sendTextQueue, WebSocketMessageType.Text, new ArraySegment<byte>(encoded, 0, encoded.Length));
		}

		private async Task SendMessage(List<ArraySegment<byte>> queue, WebSocketMessageType messageType, ArraySegment<byte> buffer)
		{
			// Return control to the calling method immediately.
			await Task.Yield();

			// Make sure we have data.
			if(buffer.Count == 0)
			{
				return;
			}

			// The state of the connection is contained in the context Items dictionary.
			bool sending;

			lock(Lock)
			{
				sending = isSending;

				// If not, we are now.
				if(!isSending)
				{
					isSending = true;
				}
			}

			if(!sending)
			{
				// Lock with a timeout, just in case.
				if(!Monitor.TryEnter(m_Socket, 1000))
				{
					// If we couldn't obtain exclusive access to the socket in one second, something is wrong.
					await m_Socket.CloseAsync(WebSocketCloseStatus.InternalServerError, string.Empty, CancellationToken.None);
					return;
				}

				try
				{
					// Send the message synchronously.
					var t = m_Socket.SendAsync(buffer, messageType, true, CancellationToken.None);
					t.Wait();
				}
				finally
				{
					Monitor.Exit(m_Socket);
				}

				// Note that we've finished sending.
				lock(Lock)
				{
					isSending = false;
				}

				// Handle any queued messages.
				await HandleQueue(queue, messageType);
			}
			else
			{
				// Add the message to the queue.
				lock(Lock)
				{
					queue.Add(buffer);
				}
			}
		}

		private async Task HandleQueue(List<ArraySegment<byte>> queue, WebSocketMessageType messageType)
		{
			var buffer = new ArraySegment<byte>();
			lock(Lock)
			{
				// Check for an item in the queue.
				if(queue.Count > 0)
				{
					// Pull it off the top.
					buffer = queue[0];
					queue.RemoveAt(0);
				}
			}

			// Send that message.
			if(buffer.Count > 0)
			{
				await SendMessage(queue, messageType, buffer);
			}
		}


		public async Task Receive()
		{
			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);

			while(m_Socket.State == System.Net.WebSockets.WebSocketState.Open)
			{
				WebSocketReceiveResult result = null;

				using(var ms = new MemoryStream())
				{
					do
					{
						result = await m_Socket.ReceiveAsync(buffer, CancellationToken.None);
						ms.Write(buffer.Array, buffer.Offset, result.Count);
					}
					while(!result.EndOfMessage);

					ms.Seek(0, SeekOrigin.Begin);

					if(result.MessageType == WebSocketMessageType.Text)
					{
						OnMessage?.Invoke(ms.ToArray());
						//using (var reader = new StreamReader(ms, Encoding.UTF8))
						//{
						//	string message = reader.ReadToEnd();
						//	OnMessage?.Invoke(this, new MessageEventArgs(message));
						//}
					}
					else if(result.MessageType == WebSocketMessageType.Binary)
					{
						OnMessage?.Invoke(ms.ToArray());
					}
					else if(result.MessageType == WebSocketMessageType.Close)
					{
						await Close();
						OnClose?.Invoke(WebSocketHelpers.ParseCloseCodeEnum((int)result.CloseStatus));
						break;
					}
				}
			}

		}

		public async Task Close()
		{
			if(State == WebSocketState.Open)
			{
				await m_Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
			}
		}
	}
}