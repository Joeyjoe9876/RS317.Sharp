using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Rs317.Sharp
{
	public sealed class OpenTKImageProducer : BaseRsImageProducer<OpenTKRsGraphicsContext>
	{
		private Bitmap image { get; }

		private FasterPixel FasterPixel { get; }

		public OpenTKImageProducer(int width, int height)
			: base(width, height)
		{
			image = new Bitmap(width, height);
			FasterPixel = new FasterPixel(image);
			initDrawingArea();
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z)
		{

		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<OpenTKRsGraphicsContext> rsGraphicsProvider)
		{
			lock(rsGraphicsProvider.SyncObj)
			{
				method239();
				rsGraphicsProvider.GameGraphics.DrawImage(image, x, y);
			}
		}

		private void method239()
		{
			lock (image)
			{
				FasterPixel.Lock();
				for(int y = 0; y < height; y++)
				{
					for(int x = 0; x < width; x++)
					{
						int value = pixels[x + y * width];
						//fastPixel.SetPixel(x, y, Color.FromArgb((value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF));
						FasterPixel.SetPixel(x, y, (byte)(value >> 16), (byte)(value >> 8), (byte)value, 255);
					}
				}
				FasterPixel.Unlock(true);
			}
		}
	}
}
