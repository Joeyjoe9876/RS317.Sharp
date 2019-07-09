using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/// <summary>
	/// Object that has a string mappable name.
	/// </summary>
	public interface INameable
	{
		/// <summary>
		/// The name of the object.
		/// </summary>
		string Name { get; }
	}
}
