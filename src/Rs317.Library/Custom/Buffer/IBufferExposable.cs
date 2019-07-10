using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IBufferExposeable
	{
		//Kept lowercase due to Java.
		byte[] buffer { get; }
	}
}
