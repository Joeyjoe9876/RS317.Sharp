using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	//Task.Delay does not work in WebGL because it uses
	//System.Timers which requires multithreading under the hood.
	//To mitigate this we manually create and managed timers on the main thread.
	public sealed class WebGLUnityTaskDelayFactory : MonoBehaviour, ITaskDelayFactory
	{
		private class RegisteredDelayTaskSource
		{
			private long CreationTime { get; }

			private long DurationTicks { get; }

			public TaskCompletionSource<bool> DelayTaskCompletionSource { get; }

			public RegisteredDelayTaskSource(long creationTime, long durationInTicks)
			{
				CreationTime = creationTime;
				DurationTicks = durationInTicks;
				DelayTaskCompletionSource = new TaskCompletionSource<bool>();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool isCompleted(long currentTime)
			{
				long ticksPassed = currentTime - CreationTime;

				return ticksPassed >= DurationTicks;
			}
		}

		private readonly object SyncObj = new object();

		private List<RegisteredDelayTaskSource> DelayTaskList = new List<RegisteredDelayTaskSource>();

		private void Update()
		{
			lock (SyncObj)
			{
				if (DelayTaskList.Count == 0)
					return;

				//Poll until they're all complete.
				while (PollRegisterTaskSourceList()) ;
			}
		}

		private bool PollRegisterTaskSourceList()
		{
			//You may wonder why I'm doing things so oddly here
			//There seemed to be some issues with completing the task within the loop.
			//might actually be incorrect IL being generated??
			RegisteredDelayTaskSource finished = null;

			bool isTaskFired = false;
			long time = DateTime.UtcNow.Ticks;
			foreach (RegisteredDelayTaskSource source in DelayTaskList)
			{
				if (source.isCompleted(time))
				{
					finished = source;
					isTaskFired = true;
					break;
				}
			}

			if (isTaskFired)
				finished.DelayTaskCompletionSource.SetResult(true);

			if (isTaskFired)
				if (DelayTaskList.Count == 0)
					DelayTaskList.Clear();
				else
					DelayTaskList.Remove(finished);

			return isTaskFired;
		}

		public Task Create(int context)
		{
			lock (SyncObj)
			{
				RegisteredDelayTaskSource task = new RegisteredDelayTaskSource(DateTime.UtcNow.Ticks, context * TimeSpan.TicksPerMillisecond);
				DelayTaskList.Add(task);
				return task.DelayTaskCompletionSource.Task;
			}
		}
	}
}
