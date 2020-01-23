using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class AsyncTaskAwaitableComponent : MonoBehaviour
	{
		public Task SetTask { get; set; }

		private async Task Start()
		{
			try
			{
				await SetTask;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed {name}. Reason: {e}");
				throw;
			}
			finally
			{
				UnityEngine.GameObject.Destroy(this.gameObject);
			}
		}
	}
}
