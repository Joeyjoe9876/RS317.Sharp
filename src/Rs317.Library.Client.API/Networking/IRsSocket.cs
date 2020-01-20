using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IRsSocket
	{
		void write(int i, byte[] abyte0);

		void read(byte[] abyte0, int j);

		int read();

		int available();

		void close();
	}
}
