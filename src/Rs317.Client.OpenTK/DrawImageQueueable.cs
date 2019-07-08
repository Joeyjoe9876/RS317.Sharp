using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class DrawImageQueueable
	{
		public int XDrawOffset { get; }

		public int XHeightOffset { get; }

		public Bitmap Image { get; }

		public int Height { get; }

		public int Width { get; }

		public DrawImageQueueable(int widthOffset, int heightOffset, Bitmap image, int width, int height)
		{
			XDrawOffset = widthOffset;
			XHeightOffset = heightOffset;
			Image = image ?? throw new ArgumentNullException(nameof(image));
			Width = width;
			Height = height;
		}
	}
}
