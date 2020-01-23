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
			private float CreationTime { get; }

			private float DurationSeconds { get; }

			public TaskCompletionSource<bool> DelayTaskCompletionSource { get; }

			public RegisteredDelayTaskSource(float creationTime, int durationInMilliseconds)
			{
				CreationTime = creationTime;
				DurationSeconds = (float)durationInMilliseconds / 1000.0f;
				DelayTaskCompletionSource = new TaskCompletionSource<bool>();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool isCompleted(float currentTime)
			{
				float secondsPassed = currentTime - CreationTime;

				return secondsPassed >= DurationSeconds;
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

				//You may wonder why I'm doing things so oddly here
				//There seemed to be some issues with completing the task within the loop.
				//might actually be incorrect IL being generated??
				RegisteredDelayTaskSource finished = null;

				foreach (RegisteredDelayTaskSource source in DelayTaskList)
				{
					if (source.isCompleted(Time.time))
					{
						finished = source;
						break;
					}
				}

				finished?.DelayTaskCompletionSource.SetResult(true);

				//DelayTaskList = DelayTaskList.Where(t => !t.DelayTaskCompletionSource.Task.IsCompleted).ToList();
				DelayTaskList.RemoveAll(t => t.DelayTaskCompletionSource.Task.IsCompleted);
			}
		}

		public Task Create(int context)
		{
			lock (SyncObj)
			{
				RegisteredDelayTaskSource task = new RegisteredDelayTaskSource(Time.time, context);
				DelayTaskList.Add(task);
				return task.DelayTaskCompletionSource.Task;
			}
		}
	}
}
