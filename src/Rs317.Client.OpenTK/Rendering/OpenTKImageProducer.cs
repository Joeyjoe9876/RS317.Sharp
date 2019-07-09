using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Rs317.Sharp
{
	public sealed class OpenTKImageProducer : BaseRsImageProducer<OpenTKRsGraphicsContext>, IOpenTKImageRenderable
	{
		private Bitmap image { get; }

		private FasterPixel FasterPixel { get; }

		public bool isDirty { get; private set; } = false;

		public IntPtr ImageDataPointer { get; }

		public Rectangle ImageLocation { get; private set; }

		public object SyncObject { get; } = new object();

		private bool accessedPixelBuffer { get; set; } = false;

		public OpenTKImageProducer(int width, int height, string name)
			: base(width, height, name)
		{
			image = new Bitmap(width, height);

			BitmapData data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, image.PixelFormat);
			ImageDataPointer = data.Scan0;
			image.UnlockBits(data);

			FasterPixel = new FasterPixel(image);
			initDrawingArea();

			ImageLocation = new Rectangle(0, 0, width, height);
		}

		public void ConsumeDirty()
		{
			lock (SyncObject)
				isDirty = false;
		}

		public override int[] pixels
		{
			get
			{
				accessedPixelBuffer = true;
				return base.pixels;
			}
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z)
		{
			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<OpenTKRsGraphicsContext> rsGraphicsProvider)
		{
			ImageLocation = new Rectangle(x, y, width, height);

			//It's only dirty if they accessed the pixel buffer.
			//TODO: We can optimize around this in the client itself.
			lock (SyncObject)
			{
				if(accessedPixelBuffer)
					isDirty = true;

				accessedPixelBuffer = false;
			}
		}

		private void method239()
		{
			FasterPixel.Lock();
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					//Important to call base pixels here, 20% of time spent
					//in this loop was due to locking on the child pixels.
					int value = base.pixels[x + y * width];
					//fastPixel.SetPixel(x, y, Color.FromArgb((value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF));
					FasterPixel.SetPixel(x, y, (byte)(value >> 16), (byte)(value >> 8), (byte)value, 255);
				}
			}
			FasterPixel.Unlock(true);
		}
	}
}
