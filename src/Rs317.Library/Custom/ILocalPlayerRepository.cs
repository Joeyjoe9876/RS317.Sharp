using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface ILocalPlayerRepository
	{
		/// <summary>
		/// The current local player of the client.
		/// </summary>
		Player LocalPlayer { get; }
	}
}
