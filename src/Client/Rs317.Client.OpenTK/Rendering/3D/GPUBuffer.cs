using System;
using System.Runtime.InteropServices;

namespace Rs317.Sharp
{
	public class GPUBuffer<T>
	{
		protected int BufferPosition { get; set; } = 0;

		private int Length { get; set; } = 65536;

		protected T[] buffer = allocateDirect(65536);

		public void flip()
		{
			BufferPosition = 0;
		}

		public void clear()
		{
			BufferPosition = 0;
		}

		public void ensureCapacity(int size)
		{
			//TODO: Don't use Marshal.
			while(Length < size)
			{
				T[] newBuffer = new T[buffer.Length * 2];
				Buffer.BlockCopy(buffer, 0, newBuffer, 0, buffer.Length * Marshal.SizeOf<T>());
				buffer = newBuffer;
				Length = newBuffer.Length;
			}
		}

		public Span<T> getBuffer()
		{
			return new Span<T>(buffer, BufferPosition, Length - BufferPosition);
		}

		public static T[] allocateDirect(int size)
		{
			return new T[size];
		}
	}
}


