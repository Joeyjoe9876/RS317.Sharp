using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IRunnableHandlerSettable
	{
		IRunnable RunnableObject { get; set; }
	}
}
