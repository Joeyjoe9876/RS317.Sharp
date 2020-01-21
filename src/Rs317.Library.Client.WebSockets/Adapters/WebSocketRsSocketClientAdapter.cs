using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class WebSocketRsSocketClientAdapter : IRsSocket
	{
		private IWebSocket InternalSocket { get; }

		//Default 5000 size similar to RsSocket
		private Queue<ArraySegment<byte>> MessageQueue { get; } = new Queue<ArraySegment<byte>>(20);

		private int PendingBytes = 0;

		private readonly object SyncObj = new object();

		private byte[] SingleByteBuffer { get; } = new byte[1];

		private ArraySegment<byte>? LeftoverArraySegment = null;

		public WebSocketRsSocketClientAdapter(IWebSocket internalSocket)
		{
			InternalSocket = internalSocket ?? throw new ArgumentNullException(nameof(internalSocket));

			InternalSocket.OnMessage += InternalSocketOnMessage;
		}

		private void InternalSocketOnMessage(ArraySegment<byte> data)
		{
			lock(SyncObj)
			{
				byte[] bytes = ArrayPool<byte>.Shared.Rent(data.Count);
				Buffer.BlockCopy(data.Array, data.Offset, bytes, 0, data.Count);
				MessageQueue.Enqueue(new ArraySegment<byte>(bytes));
				PendingBytes += data.Count;
			}
		}

		public void write(int i, byte[] abyte0)
		{
			InternalSocket.Send(abyte0, 0, i);
		}

		public void read(byte[] abyte0, int j)
		{
			try
			{
				if(PendingBytes < j)
					InternalSocket.Receive();

				int i = 0; // was parameter
				int k;
				for(; j > 0; j -= k)
				{
					lock(SyncObj)
					{
						if (MessageQueue.Count > 0 || LeftoverArraySegment.HasValue)
						{
							ArraySegment<byte> bytes = LeftoverArraySegment.HasValue ? LeftoverArraySegment.Value : MessageQueue.Peek();
							
							if (bytes.Count == j || j > bytes.Count) //if it's exactly enough
							{
								Buffer.BlockCopy(bytes.Array, bytes.Offset, abyte0, i, bytes.Count);
								ArrayPool<byte>.Shared.Return(bytes.Array);

								//Depends on the source
								if (LeftoverArraySegment.HasValue)
									LeftoverArraySegment = null;
								else
									MessageQueue.Dequeue();

								//Used all bytes (could be more than we read,
								k = bytes.Count;
							}
							else// if (j < bytes.Count)
							{
								//We have to do this before, if we DON'T have a pending segment
								//to process then we dequeue because we've actually peeked this one we're referencing in
								//the case where we do have one is where it's null.
								if(!LeftoverArraySegment.HasValue)
									MessageQueue.Dequeue();

								//We have TO MANY bytes and so we'll have some left over.
								Buffer.BlockCopy(bytes.Array, bytes.Offset, abyte0, i, j);
								LeftoverArraySegment = new ArraySegment<byte>(bytes.Array, bytes.Offset + j, bytes.Count - j);

								//Used all bytes technically
								k = j;
							}

							PendingBytes -= k;
						}
						else
							k = 0;
					}

					i += k;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"Internal WebSocket Read Multiple Error: {e}");
				throw;
			}
		}

		public int read()
		{
			try
			{
				read(SingleByteBuffer, 1);
				return SingleByteBuffer[0];
			}
			catch (Exception e)
			{
				Console.WriteLine($"Internal WebSocket Read Error: {e}");
				throw;
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
