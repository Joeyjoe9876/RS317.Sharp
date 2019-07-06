using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317
{
	public interface IRunnableStarter
	{
		void StartRunnable(IRunnable runnable, int priority);
	}
}
