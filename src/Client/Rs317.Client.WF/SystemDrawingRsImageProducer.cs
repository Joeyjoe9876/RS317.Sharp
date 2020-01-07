using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Rs317.Sharp
{
	public sealed class SystemDrawingRsImageProducer : BaseRsImageProducer<Graphics>
	{
		private Bitmap image { get; }

		private IntPtr ImageDataPointer { get; }

		public SystemDrawingRsImageProducer(int width, int height, string name)
			: base(width, height, name)
		{
			image = new Bitmap(width, height);

			BitmapData data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, image.PixelFormat);
			ImageDataPointer = data.Scan0;
			image.UnlockBits(data);

			initDrawingArea();
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z, IRSGraphicsProvider<Graphics> graphicsObject)
		{
			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<Graphics> rsGraphicsProvider, bool force = false)
		{
			lock(rsGraphicsProvider.SyncObj)
			{
				rsGraphicsProvider.GameGraphics.DrawImageUnscaled(image, x, y);
			}
		}

		private unsafe void method239()
		{
			int* pointer = (int*)ImageDataPointer;

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int index = (x + y * width);
					int value = base.pixels[index];

					//The 255 << 24 is the alpha bits that must be set.
					*(pointer + index) = value + (255 << 24);
				}
			}
		}
	}
}
