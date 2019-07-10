using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IBufferSeekable
	{
		//Kept lowercase for Java for now.
		int position { get; set; }
	}
}
