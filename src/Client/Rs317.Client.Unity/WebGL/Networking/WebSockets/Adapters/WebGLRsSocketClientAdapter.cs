using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace Rs317.Sharp
{
	public sealed class WebGLRsSocketClientAdapter : IRsSocket
	{
		private IWebSocket InternalSocket { get; }

		//Default 5000 size similar to RsSocket
		private MemoryStream InternalStream { get; } = new MemoryStream(5000);

		private int PendingBytes = 0;

		private readonly object SyncObj = new object();

		public WebGLRsSocketClientAdapter([NotNull] IWebSocket internalSocket)
		{
			InternalSocket = internalSocket ?? throw new ArgumentNullException(nameof(internalSocket));

			InternalSocket.OnMessage += InternalSocketOnMessage;
		}

		private void InternalSocketOnMessage(byte[] data)
		{
			lock (SyncObj)
			{
				//If we have no pending bytes then we can finally take this oppurtunity to reset the stream position.
				if (PendingBytes == 0)
					InternalStream.Position = 0;

				PendingBytes += data.Length;
				InternalStream.Write(data, 0, data.Length);
			}
		}

		public void write(int i, byte[] abyte0)
		{
			InternalSocket.Send(abyte0, 0, i);
		}

		public void read(byte[] abyte0, int j)
		{
			int totalBytesRead = 0; // was parameter
			for(int amountRead = 0; j > 0 && InternalSocket.State == WebSocketState.Open; j -= amountRead)
			{
				lock (SyncObj)
				{
					int read = InternalStream.Read(abyte0, totalBytesRead, j - amountRead);
					PendingBytes -= read;
					amountRead += read;
				}

				if(amountRead <= 0)
					throw new IOException("EOF");

				totalBytesRead += amountRead;
			}
		}

		public int read()
		{
			lock (SyncObj)
			{
				if (PendingBytes == 0)
					return 0;
				
				PendingBytes--;
				return InternalStream.ReadByte();
			}
		}

		public int available()
		{
			lock(SyncObj)
				return (int) PendingBytes;
		}

		public void close()
		{
			InternalSocket.Close();
		}
	}
}
