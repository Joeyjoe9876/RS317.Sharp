using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class DefaultTaskDelayFactory : ITaskDelayFactory
	{
		public Task Create(int context)
		{
			return Task.Delay(context);
		}
	}
}
