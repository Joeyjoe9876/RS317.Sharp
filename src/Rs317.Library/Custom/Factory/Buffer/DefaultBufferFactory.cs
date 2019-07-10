using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class DefaultBufferFactory : IBufferFactory
	{
		public IBuffer Create(EmptyFactoryCreationContext context)
		{
			//This is how the old Buffer.Create functioned, except it would try to pull from a cache. Which was dumb.
			return new Default317Buffer(new byte[5000]);
		}
	}
}
