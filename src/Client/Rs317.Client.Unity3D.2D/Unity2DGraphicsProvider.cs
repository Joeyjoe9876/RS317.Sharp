using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	public class Unity2DGraphicsProvider : IRSGraphicsProvider<Graphics>
	{
		/// <summary>
		/// The system drawing GameGraphics object.
		/// From the original Java client.
		/// </summary>
		public Graphics GameGraphics { get; }

		public object SyncObj { get; } = new object();

		public Unity2DGraphicsProvider(Graphics gameGraphics)
		{
			GameGraphics = gameGraphics ?? throw new ArgumentNullException(nameof(gameGraphics));
		}
	}
}
