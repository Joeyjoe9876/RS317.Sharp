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
			await RunnableObject.run();
		}
	}
}
