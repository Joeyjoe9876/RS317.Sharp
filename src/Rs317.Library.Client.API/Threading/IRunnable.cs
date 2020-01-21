using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	//TODO: Create namespace.
	/// <summary>
	/// Based on the Java runnable.
	/// </summary>
	public interface IRunnable
	{
		//Lowecase to match java.
		/// <summary>
		/// The run method of the runnable.
		/// </summary>
		Task run();
	}
}
