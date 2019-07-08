using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class SystemDrawingRsGraphicsAdapter : IRSGraphicsProvider<Graphics>
	{
		/// <summary>
		/// The system drawing GameGraphics object.
		/// From the original Java client.
		/// </summary>
		public Graphics GameGraphics { get; }

		public object SyncObj { get; } = new object();

		public SystemDrawingRsGraphicsAdapter(Graphics gameGraphics)
		{
			GameGraphics = gameGraphics ?? throw new ArgumentNullException(nameof(gameGraphics));
		}
	}
}
