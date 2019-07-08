using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public class OpenTKRsGraphicsProvider : IRSGraphicsProvider<OpenTKRsGraphicsContext>
	{
		/// <summary>
		/// The open TK GameGraphics object.
		/// From the original Java client.
		/// </summary>
		public OpenTKRsGraphicsContext GameGraphics { get; }

		public object SyncObj { get; } = new object();

		public OpenTKRsGraphicsProvider(OpenTKRsGraphicsContext gameGraphics)
		{
			GameGraphics = gameGraphics ?? throw new ArgumentNullException(nameof(gameGraphics));
		}
	}
}
