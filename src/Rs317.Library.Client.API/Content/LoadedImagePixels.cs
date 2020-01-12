using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class LoadedImagePixels
	{
		public int Height { get; }

		public int Width { get; }

		public int[] Pixels { get; }

		public LoadedImagePixels(int height, int width, int[] pixels)
		{
			if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
			if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));

			Height = height;
			Width = width;
			Pixels = pixels ?? throw new ArgumentNullException(nameof(pixels));
		}
	}
}
