using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class WebGLSocketThreadRunnableStarterStrategy : IRunnableStarter
	{
		private UpdateTickRunnableComponent CurrentRunnableHandler { get; set; } = null;

		public void StartRunnable(IRunnable runnable, int priority)
		{
			if(CurrentRunnableHandler != null)
				UnityEngine.GameObject.Destroy(CurrentRunnableHandler.gameObject);

			CurrentRunnableHandler = new UnityEngine.GameObject("WebGL Socket Runnable Handler").AddComponent<UpdateTickRunnableComponent>();
			CurrentRunnableHandler.RunnableObject = runnable;
		}
	}
}
