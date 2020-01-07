using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class UnityRsGraphicsProvider : IRSGraphicsProvider<UnityRsGraphics>
	{
		/// <summary>
		/// The system drawing GameGraphics object.
		/// From the original Java client.
		/// </summary>
		public UnityRsGraphics GameGraphics { get; }

		public object SyncObj { get; } = new object();

		public UnityRsGraphicsProvider(UnityRsGraphics gameGraphics)
		{
			GameGraphics = gameGraphics ?? throw new ArgumentNullException(nameof(gameGraphics));
			gameGraphics.SyncObj = SyncObj;
		}
	}
}
