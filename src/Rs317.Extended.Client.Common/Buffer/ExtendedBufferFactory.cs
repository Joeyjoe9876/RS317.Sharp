using System;
using System.Collections.Generic;
using System.Text;
using Rs317.Sharp;

namespace Rs317.Extended
{
	public sealed class ExtendedBufferFactory : IBufferFactory
	{
		public IBuffer Create(EmptyFactoryCreationContext context)
		{
			return new ExtendedRs317Buffer(new byte[5000]);
		}
	}
}
