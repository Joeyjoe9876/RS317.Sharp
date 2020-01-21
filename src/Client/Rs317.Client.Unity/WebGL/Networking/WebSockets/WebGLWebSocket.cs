using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	/// <summary>
	/// WebSocket class bound to JSLIB.
	/// </summary>
	public class WebGLWebSocket : IWebSocket
	{
		/* WebSocket JSLIB functions */
		[DllImport("__Internal")]
		public static extern int WebSocketConnect(int instanceId);

		[DllImport("__Internal")]
		public static extern int WebSocketClose(int instanceId, int code, string reason);

		[DllImport("__Internal")]
		public static extern unsafe int WebSocketSend(int instanceId, byte* dataPtr, int dataLength);

		[DllImport("__Internal")]
		public static extern int WebSocketGetState(int instanceId);

		protected int instanceId;

		public event WebSocketOpenEventHandler OnOpen;

		public event WebSocketMessageEventHandler OnMessage;

		public event WebSocketErrorEventHandler OnError;

		public event WebSocketCloseEventHandler OnClose;

		public WebGLWebSocket(int instanceId)
		{
			this.instanceId = instanceId;
		}

		~WebGLWebSocket()
		{
			WebGLWebSocketFactory.HandleInstanceDestroy(this.instanceId);
		}

		public int GetInstanceId()
		{
			return this.instanceId;
		}

		private Task Connect()
		{
			int ret = WebSocketConnect(this.instanceId);

			if (ret < 0)
				throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

			return Task.CompletedTask;
		}

		public Task Close(WebSocketCloseCode code = WebSocketCloseCode.Normal, string reason = null)
		{
			int ret = WebSocketClose(this.instanceId, (int)code, reason);

			if (ret < 0)
				throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);

			return Task.CompletedTask;
		}

		public Task Receive()
		{
			return Task.CompletedTask;
		}

		public async Task<bool> ConnectAsync(SocketCreationContext connectionInfo)
		{
			await Connect();
			return this.State == WebSocketState.Connecting || this.State == WebSocketState.Open;
		}

		public unsafe Task Send(byte[] data, int offset, int length)
		{
			fixed (byte* ptr = &data[offset])
			{
				int ret = WebSocketSend(this.instanceId, ptr, length);

				if(ret < 0)
					throw WebSocketHelpers.GetErrorMessageFromCode(ret, null);
			}

			return Task.CompletedTask;
		}

		public WebSocketState State {
			get {
				int state = WebSocketGetState(this.instanceId);

				if (state < 0)
					throw WebSocketHelpers.GetErrorMessageFromCode(state, null);

				switch (state)
				{
					case 0:
						return WebSocketState.Connecting;

					case 1:
						return WebSocketState.Open;

					case 2:
						return WebSocketState.Closing;

					case 3:
						return WebSocketState.Closed;

					default:
						return WebSocketState.Closed;
				}
			}
		}

		public void DelegateOnOpenEvent()
		{
			this.OnOpen?.Invoke();
		}

		public void DelegateOnMessageEvent(byte[] data)
		{
			this.OnMessage?.Invoke(new ArraySegment<byte>(data));
		}

		public void DelegateOnErrorEvent(string errorMsg)
		{
			this.OnError?.Invoke(errorMsg);
		}

		public void DelegateOnCloseEvent(int closeCode)
		{
			this.OnClose?.Invoke(WebSocketHelpers.ParseCloseCodeEnum(closeCode));
		}
	}
}