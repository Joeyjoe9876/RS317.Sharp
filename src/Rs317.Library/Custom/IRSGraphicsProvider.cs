using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/// <summary>
	/// Contract for types that expose graphics functionality.
	/// </summary>
	public interface IRSGraphicsProvider<out TGraphicsObjectType>
	{
		/// <summary>
		/// The syncronization object for the graphics provide.
		/// </summary>
		object SyncObj { get; }

		TGraphicsObjectType GameGraphics { get; }
	}
}
