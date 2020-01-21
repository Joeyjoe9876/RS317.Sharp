using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Fleck;

namespace Rs317.Sharp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			int mainPort = 43594;

			//Connecting to Server: 127.0.0.1:43594
			WebSocketServer server = new WebSocketServer($"ws://127.0.0.1:{mainPort + 1}");
			ConcurrentDictionary<Guid, TcpClient> TcpClientDictionary = new ConcurrentDictionary<Guid, TcpClient>();
			server.ListenerSocket.NoDelay = true;
			server.Start(socket =>
			{
				socket.OnOpen = () =>
				{
					Console.WriteLine($"Open! Id: {socket.ConnectionInfo.Id}");
					TcpClient tcpClient = new TcpClient();
					TcpClientDictionary.TryAdd(socket.ConnectionInfo.Id, tcpClient);
					Task.Factory.StartNew(async () => { await OnConnectionOpenAsync(tcpClient, mainPort, TcpClientDictionary, socket); });
				};

				socket.OnError = e =>
				{
					Console.WriteLine($"Close! Error: {e.Message}");
					socket.Close();
				};

				socket.OnClose = () => { OnConnectionClose(TcpClientDictionary, socket); };

				socket.OnBinary = (byte[] bytes) => { OnBinaryMessage(bytes, TcpClientDictionary, socket); };
			});

			while (true)
				await Task.Delay(10000);
		}

		private static void OnBinaryMessage(byte[] bytes, ConcurrentDictionary<Guid, TcpClient> TcpClientDictionary, IWebSocketConnection socket)
		{
			Console.WriteLine($"Client Sent Bytes: {bytes.Length}");

			TcpClientDictionary[socket.ConnectionInfo.Id].GetStream().Write(bytes);
		}

		private static void OnConnectionClose(ConcurrentDictionary<Guid, TcpClient> TcpClientDictionary, IWebSocketConnection socket)
		{
			Console.WriteLine("Close!");
			TcpClientDictionary[socket.ConnectionInfo.Id].Close();
			TcpClientDictionary.TryRemove(socket.ConnectionInfo.Id, out var val);
		}

		private static async Task OnConnectionOpenAsync(TcpClient tcpClient, int mainPort, ConcurrentDictionary<Guid, TcpClient> TcpClientDictionary, IWebSocketConnection socket)
		{
			try
			{
				await tcpClient.ConnectAsync("127.0.0.1", mainPort);

				if (!tcpClient.Connected)
					throw new InvalidOperationException($"Failed to connect to RS server.");

				NetworkStream stream = tcpClient.GetStream();
				byte[] buffer = new byte[5000];
				while (tcpClient.Connected && TcpClientDictionary.ContainsKey(socket.ConnectionInfo.Id))
				{
					int byteCountRead = await stream.ReadAsync(buffer);

					if (byteCountRead == 0)
						continue;

					Console.WriteLine($"Server Sent Bytes: {byteCountRead}");

					//TODO: Optimize this.
					await socket.Send(new ArraySegment<byte>(buffer, 0, byteCountRead).ToArray());
				}

				Console.WriteLine($"Server proxy listener stopped.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Listener failed. Reason: {e}");
				throw;
			}
		}
	}
}
