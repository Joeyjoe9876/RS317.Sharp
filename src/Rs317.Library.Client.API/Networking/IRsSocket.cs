using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public interface IRsSocket
	{
		Task write(int i, byte[] abyte0);

		Task read(byte[] abyte0, int j);

		Task<int> read();

		int available();

		void close();

		Task<bool> Connect(SocketCreationContext socketConnectionContext);
	}
}
