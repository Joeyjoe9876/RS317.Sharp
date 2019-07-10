using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class DefaultBufferFactory : IBufferFactory
	{
		public IBuffer Create(EmptyFactoryCreationContext context)
		{
			//Copied from old buffer.create
			Buffer stream_1 = new Buffer();
			stream_1.position = 0;
			stream_1.buffer = new byte[5000];
			return stream_1;
		}
	}
}
