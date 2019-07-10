using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class EmptyFactoryCreationContext
	{
		public static EmptyFactoryCreationContext Instance { get; } = new EmptyFactoryCreationContext();

		private EmptyFactoryCreationContext()
		{
			
		}
	}
}
