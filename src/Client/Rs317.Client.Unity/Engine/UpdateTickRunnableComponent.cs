using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class UpdateTickRunnableComponent : MonoBehaviour
	{
		public IRunnable RunnableObject { get; set; }

		void Start()
		{
			if(RunnableObject == null)
				throw new InvalidOperationException($"{nameof(RunnableObject)} cannot be null in {GetType().Name}");
		}

		void Update()
		{
			RunnableObject.run();
		}
	}
}
