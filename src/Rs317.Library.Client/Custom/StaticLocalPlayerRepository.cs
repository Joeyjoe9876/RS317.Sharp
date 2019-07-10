using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	//TODO: This is so bad, no good way to share the player instance cross project...
	//I really hate this design, I need IoC. But, the original client is ridiculously poor to work with.
	//So, we  have to do hacky things like this
	public class StaticLocalPlayerRepository : ILocalPlayerRepository
	{
		//Exposed so the Client can se it.
		public static Player LocalPlayerInstance { get; set; }

		public Player LocalPlayer => LocalPlayerInstance;
	}
}
