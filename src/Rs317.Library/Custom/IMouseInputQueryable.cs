using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317
{
	//Basically, this exists to seperate the MouseDetection class from a dependency on the Client Type.
	/// <summary>
	/// Conctract for a type that exposes mouse information/data.
	/// </summary>
	public interface IMouseInputQueryable
	{
		//Lowercast to match Java
		int mouseX { get; }

		int mouseY { get; }
	}
}
