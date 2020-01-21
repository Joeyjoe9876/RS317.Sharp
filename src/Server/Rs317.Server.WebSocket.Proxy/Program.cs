using System;
using System.Collections.Concurrent;
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

			server.Start(socket =>
			{
				socket.OnOpen = () =>
				{
					Console.WriteLine("Open!");
					TcpClientDictionary.TryAdd(socket.ConnectionInfo.Id, new TcpClient("127.0.0.1", mainPort));
				};

				socket.OnClose = () =>
				{
					Console.WriteLine("Close!");
					TcpClientDictionary[socket.ConnectionInfo.Id].Close();
				};

				socket.OnBinary = (byte[] bytes) => TcpClientDictionary[socket.ConnectionInfo.Id].GetStream().Write(bytes);
			});

			while (true)
				await Task.Delay(10000);
		}
	}
}
