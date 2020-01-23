using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class WebGLRunnableStarterStrategy : IRunnableStarter
	{
		public void StartRunnable(IRunnable runnable, int priority)
		{
			AsyncStartRunnableComponent asyncRunnableComponent = new UnityEngine.GameObject($"Runnable: {runnable.GetType().Name}").AddComponent<AsyncStartRunnableComponent>();
			asyncRunnableComponent.RunnableObject = runnable;
		}
	}
}
