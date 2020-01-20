using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;

namespace Rs317.Sharp
{
	public sealed class WebGLTcpClientRsSocket : IRsSocket
	{
		private TcpClient InternalClient { get; }

		private NetworkStream InternalClientStream { get; }

		public WebGLTcpClientRsSocket([NotNull] TcpClient internalClient)
		{
			InternalClient = internalClient ?? throw new ArgumentNullException(nameof(internalClient));
			InternalClientStream = internalClient.GetStream();

			InternalClient.SendTimeout = 30000;
			InternalClient.NoDelay = true;
		}

		public void write(int i, byte[] abyte0)
		{
			if (!InternalClient.Connected)
				return;

			InternalClientStream.Write(abyte0, 0, i);
		}

		public void read(byte[] abyte0, int j)
		{
			int i = 0; // was parameter
			if(!InternalClient.Connected)
				return;

			int k;
			for(; j > 0; j -= k)
			{
				k = InternalClientStream.Read(abyte0, i, j);
				if(k <= 0)
					throw new IOException("EOF");
				i += k;
			}
		}

		public int read()
		{
			if(!InternalClient.Connected)
				return 0;
			else
				return InternalClientStream.ReadByte();
		}

		public int available()
		{
			if(!InternalClient.Connected)
				return 0;
			else
				return InternalClient.Available;
		}

		public void close()
		{
			InternalClientStream.Dispose();
			InternalClient.Dispose();
		}
	}
}
