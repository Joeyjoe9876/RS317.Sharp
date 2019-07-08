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

		public DrawImageQueueable(int width, int height, Bitmap image)
		{
			XDrawOffset = width;
			XHeightOffset = height;
			Image = image ?? throw new ArgumentNullException(nameof(image));
		}
	}
}
