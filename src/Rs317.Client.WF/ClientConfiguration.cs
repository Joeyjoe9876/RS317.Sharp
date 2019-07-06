using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317
{
	public sealed class ClientConfiguration
	{
		public int WorldId { get; }

		public short Port { get; }

		public bool isMembersWorld { get; }

		public bool isLowMemory { get; }

		public ClientConfiguration(int worldId, short port, bool isMembersWorld)
		{
			WorldId = worldId;
			Port = port;
			this.isMembersWorld = isMembersWorld;

			//The C# client doesn't support low memory mode.
			isLowMemory = false;
		}
	}
}
