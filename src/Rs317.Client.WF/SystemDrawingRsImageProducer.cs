using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Rs317.Sharp
{
	public sealed class SystemDrawingRsImageProducer : BaseRsImageProducer<Graphics>
	{
		private Bitmap image { get; }

		private FasterPixel fastPixel { get; }

		public SystemDrawingRsImageProducer(int width, int height)
			: base(width, height)
		{
			image = new Bitmap(width, height);
			fastPixel = new FasterPixel(image);
			initDrawingArea();
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z)
		{
			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<Graphics> rsGraphicsProvider)
		{
			lock(rsGraphicsProvider.SyncObj)
			{
				rsGraphicsProvider.GameGraphics.DrawImageUnscaled(image, x, y);
			}
		}

		private void method239()
		{
			fastPixel.Lock();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int value = pixels[x + y * width];
					//fastPixel.SetPixel(x, y, Color.FromArgb((value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF));
					fastPixel.SetPixel(x, y, (byte)(value >> 16), (byte)(value >> 8), (byte)value, 255);
				}
			}
			fastPixel.Unlock(true);
		}
	}
}
