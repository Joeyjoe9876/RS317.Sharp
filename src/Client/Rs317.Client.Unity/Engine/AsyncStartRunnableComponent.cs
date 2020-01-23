using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class AsyncStartRunnableComponent : MonoBehaviour, IRunnableHandlerSettable
	{
		public IRunnable RunnableObject { get; set; }

		private async Task Start()
		{
			try
			{
				await RunnableObject.run();

				//Once the runnable is done we don't need this.
				UnityEngine.GameObject.Destroy(this.gameObject);
			}
			catch (Exception e)
			{
				Console.WriteLine($"AsyncStartRunnable Failed. Runnable: {RunnableObject?.GetType().Name} Reason: {e}");
				throw;
			}
		}
	}
}
