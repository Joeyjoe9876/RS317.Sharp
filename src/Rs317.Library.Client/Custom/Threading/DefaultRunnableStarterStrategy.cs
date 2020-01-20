using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class DefaultRunnableStarterStrategy : IRunnableStarter
	{
		public void StartRunnable(IRunnable runnable, int priority)
		{
			//Run it on the threadpool instead.
			Task.Factory.StartNew(runnable.run, priority < 1 ? TaskCreationOptions.LongRunning : TaskCreationOptions.None);
		}
	}
}
