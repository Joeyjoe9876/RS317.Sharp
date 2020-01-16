using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public static class RsClientConstants
	{
		public const long RsClientTicksInTicks = TimeSpan.TicksPerMillisecond * RsClientMillisecondsPerTicks;

		public const long RsClientMillisecondsPerTicks = 600;
	}
}
