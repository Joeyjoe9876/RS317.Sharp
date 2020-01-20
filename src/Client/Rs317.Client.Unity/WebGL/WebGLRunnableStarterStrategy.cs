using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class WebGLRunnableStarterStrategy : IRunnableStarter
	{
		public void StartRunnable(IRunnable runnable, int priority)
		{
			throw new InvalidOperationException($"WebGL doesn't support manual startables.");
		}
	}
}
