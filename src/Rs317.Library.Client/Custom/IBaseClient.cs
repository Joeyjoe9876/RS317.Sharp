using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public interface IBaseClient
	{
		FileCache[] caches { get; set; }

		void startRunnable(IRunnable runnable, int priority);

		Task<TcpClient> openSocketAsync(int port);

		bool isLoggedIn { get; }

		int CurrentTick { get; }

		int GetInterfaceSettings(int index);
	}
}
