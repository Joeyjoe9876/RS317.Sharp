using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class RSSocket : IRunnable, IRsSocket
	{
		private NetworkStream inputStream;

		private NetworkStream outputStream;

		private TcpClient socket;

		private bool closed;

		private IRunnableStarter RunnableStarterService { get; }

		private byte[] buffer;

		private int writeIndex;

		private int buffIndex;

		private bool isWriter;
		private bool hasIOError;

		private byte[] SingleByteBuffer { get; } = new byte[1];

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		public RSSocket(IRunnableStarter runnablerStarter, TcpClient socket1)
		{
			closed = false;
			isWriter = false;
			hasIOError = false;
			RunnableStarterService = runnablerStarter;
			socket = socket1;
			socket.SendTimeout = 30000;
			socket.NoDelay = true;
			inputStream = socket.GetStream();
			outputStream = socket.GetStream();
		}

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		public int available()
		{
			if (closed)
				return 0;
			else
				return socket.Available;
		}

		public void close()
		{
			closed = true;
			try
			{
				if (inputStream != null)
					inputStream.Close();
				if (outputStream != null)
					outputStream.Close();
				if (socket != null)
					socket.Close();
			}
			catch (Exception _ex)
			{
				throw new InvalidOperationException($"Error closing stream. Error: {_ex.Message}", _ex);
			}

			isWriter = false;

			//Prevent runnable thread from hanging.
			lock (this)
			{
				Monitor.PulseAll(this);
			}

			buffer = null;
		}

		//Default implementation is to just return, caller should connect the socket.
		public Task<bool> Connect(SocketCreationContext socketConnectionContext)
		{
			return Task.FromResult(true);
		}

		public void printDebug()
		{
			Console.WriteLine("dummy:" + closed);
			Console.WriteLine("tcycl:" + writeIndex);
			Console.WriteLine("tnum:" + buffIndex);
			Console.WriteLine("writer:" + isWriter);
			Console.WriteLine("ioerror:" + hasIOError);
			try
			{
				Console.WriteLine("available:" + available());
			}
			catch (IOException _ex)
			{
			}
		}

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		public async Task<int> read()
		{
			if (closed)
				return 0;
			else
			{
				await read(SingleByteBuffer, 1);
				return SingleByteBuffer[0];
			}
		}

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		public async Task read(byte[] abyte0, int j)
		{
			int i = 0; // was parameter
			if (closed)
				return;
			int k;
			for (; j > 0; j -= k)
			{
				k = await inputStream.ReadAsync(abyte0, i, j);
				if (k <= 0)
					throw new IOException("EOF");
				i += k;
			}

		}

		public async Task run()
		{
			while (isWriter)
			{
				int i;
				int j;
				lock (this)
				{
					if (buffIndex == writeIndex)
						try
						{
							Monitor.Wait(this);
						}
						catch (Exception _ex)
						{
						}

					if (!isWriter)
						return;

					j = writeIndex;
					if (buffIndex >= writeIndex)
						i = buffIndex - writeIndex;
					else
						i = 5000 - writeIndex;
				}

				if (i > 0)
				{
					try
					{
						await outputStream.WriteAsync(buffer, j, i);
					}
					catch (IOException _ex)
					{
						hasIOError = true;
					}

					writeIndex = (writeIndex + i) % 5000;
					try
					{
						if (buffIndex == writeIndex)
							await outputStream.FlushAsync();
					}
					catch (IOException _ex)
					{
						hasIOError = true;
					}
				}
			}

			return;
		}

		//TODO: Add exception documentation
		/// <summary>
		/// 
		/// </summary>
		/// <exception cref=""></exception>
		/// <returns></returns>
		public Task write(int i, byte[] abyte0)
		{
			if (closed)
				return Task.CompletedTask;

			if (hasIOError)
			{
				hasIOError = false;
				throw new IOException("Error in writer thread");
			}

			if (buffer == null)
				buffer = new byte[5000];
			lock (this)
			{
				for (int l = 0; l < i; l++)
				{
					buffer[buffIndex] = abyte0[l];
					buffIndex = (buffIndex + 1) % 5000;
					if (buffIndex == (writeIndex + 4900) % 5000)
						throw new IOException("buffer overflow");
				}

				if (!isWriter)
				{
					isWriter = true;
					RunnableStarterService.StartRunnable(this, 3);
				}

				Monitor.PulseAll(this);
			}

			return Task.CompletedTask;
		}
	}
}
