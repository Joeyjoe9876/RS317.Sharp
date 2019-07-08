using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace Rs317.Sharp
{
	public sealed class FasterPixel
	{
		private readonly byte[] rgbValues;
		private BitmapData bmpData;
		private IntPtr bmpPtr;

		public int Width { get; }

		public int Height { get; }

		public bool IsAlphaBitmap { get; }

		public Bitmap Bitmap { get; }

		public FasterPixel(Bitmap bitmap)
		{
			if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

			if((bitmap.PixelFormat == (bitmap.PixelFormat | PixelFormat.Indexed)))
				throw new Exception("Cannot lock an Indexed image.");

			Bitmap = bitmap;
			IsAlphaBitmap = (Bitmap.PixelFormat == (Bitmap.PixelFormat | PixelFormat.Alpha));
			Width = bitmap.Width;
			Height = bitmap.Height;
			rgbValues = new byte[Width * Height * 4];
		}

		public void Lock()
		{
			Rectangle rect = new Rectangle(0, 0, Width, Height);
			bmpData = Bitmap.LockBits(rect, ImageLockMode.ReadWrite, Bitmap.PixelFormat);
			bmpPtr = bmpData.Scan0;
		}

		public void Unlock(bool setPixels)
		{
			// Copy the RGB values back to the bitmap
			if(setPixels) System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bmpPtr, rgbValues.Length);
			// Unlock the bits.
			Bitmap.UnlockBits(bmpData);
		}

		public void SetPixel(int x, int y, byte red, byte green, byte blue, byte alpha)
		{
			if(IsAlphaBitmap)
			{
				int index = ((y * Width + x) * 4);
				rgbValues[index] = blue;
				rgbValues[index + 1] = green;
				rgbValues[index + 2] = red;
				rgbValues[index + 3] = alpha;
			}
			else
			{
				int index = ((y * Width + x) * 3);
				rgbValues[index] = blue;
				rgbValues[index + 1] = green;
				rgbValues[index + 2] = red;
			}
		}
	}
}
