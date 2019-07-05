using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Rs317
{
	public interface IBaseClient
	{
		FileCache[] caches { get; set; }

		void startRunnable(IRunnable runnable, int priority);

		Task<TcpClient> openSocket(int port);

		bool isLoggedIn { get; }
	}
}
