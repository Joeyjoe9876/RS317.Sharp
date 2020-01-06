using System;
using System.Runtime.InteropServices;

namespace Rs317.Sharp
{
	public class GpuIntBuffer : GPUBuffer<int>
	{
		void put(int x, int y, int z, int c)
		{
			buffer[BufferPosition] = x;
			BufferPosition++;

			buffer[BufferPosition] = y;
			BufferPosition++;

			buffer[BufferPosition] = z;
			BufferPosition++;

			buffer[BufferPosition] = c;
			BufferPosition++;
		}
	}
}


