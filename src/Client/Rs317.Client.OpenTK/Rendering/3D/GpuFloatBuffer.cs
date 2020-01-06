using System;
using System.Runtime.InteropServices;

namespace Rs317.Sharp
{
	public class GpuFloatBuffer : GPUBuffer<float>
	{
		public void put(float texture, float u, float v)
		{
			buffer[BufferPosition] = texture;
			BufferPosition++;

			buffer[BufferPosition] = u;
			BufferPosition++;

			buffer[BufferPosition] = v;
			BufferPosition++;

			buffer[BufferPosition] = 0.0f;
			BufferPosition++;
		}
	}
}


