using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rs317.Sharp
{
	public sealed class OpenTKImageProducer : BaseRsImageProducer<OpenTKRsGraphicsContext>, IOpenTKImageRenderable
	{
		private Bitmap image { get; }

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
				//There is a race condition here. But it's for significant performance benefit.
				accessedPixelBuffer = true;
				return base.pixels;
			}
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z, IRSGraphicsProvider<OpenTKRsGraphicsContext> graphicsObject)
		{
			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<OpenTKRsGraphicsContext> rsGraphicsProvider, bool force = false)
		{
			ImageLocation = new Rectangle(x, y, width, height);

			//It's only dirty if they accessed the pixel buffer.
			//TODO: We can optimize around this in the client itself.
			lock (SyncObject)
			{
				if (accessedPixelBuffer)
				{
					//There is a race condition here. But it's for significant performance benefit.
					accessedPixelBuffer = false;
					isDirty = true;
				}
				else if(force)
					isDirty = true;
			}
		}

		private unsafe void method239()
		{
			Marshal.Copy(base.pixels, 0, ImageDataPointer, base.pixels.Length);
		}
	}
}
