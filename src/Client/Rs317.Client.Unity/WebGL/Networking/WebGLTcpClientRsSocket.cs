using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Rs317.Sharp
{
	public sealed class WebGLTcpClientRsSocket : IRsSocket
	{
		private TcpClient InternalClient { get; }

		private NetworkStream InternalClientStream { get; }

		private byte[] SingleByteBuffer { get; } = new byte[1];

		public WebGLTcpClientRsSocket([NotNull] TcpClient internalClient)
		{
			InternalClient = internalClient ?? throw new ArgumentNullException(nameof(internalClient));
			InternalClientStream = internalClient.GetStream();

			InternalClient.SendTimeout = 30000;
			InternalClient.NoDelay = true;
		}

		public Task write(int i, byte[] abyte0)
		{
			if (!InternalClient.Connected)
				return Task.CompletedTask;

			return InternalClientStream.WriteAsync(abyte0, 0, i);
		}

		public async Task read(byte[] abyte0, int j)
		{
			int i = 0; // was parameter
			if (!InternalClient.Connected)
				return;

			int k;
			for(; j > 0; j -= k)
			{
				k = await InternalClientStream.ReadAsync(abyte0, i, j);
				if(k <= 0)
					throw new IOException("EOF");
				i += k;
			}
		}

		public async Task<int> read()
		{
			if (!InternalClient.Connected)
				return -1;
			else
			{
				await read(SingleByteBuffer, 1);
				return SingleByteBuffer[0];
			}
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

		//We were passed a TcpClient that is assumed to be connected already
		public async Task<bool> Connect(SocketCreationContext socketConnectionContext)
		{
			return true;
		}
	}
}
