using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class ImageProducerFactoryCreationContext
	{
		public int Width { get; }

		public int Height { get; }

		public string Name { get; }

		public ImageProducerFactoryCreationContext(int width, int height, string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
			if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
			if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

			Width = width;
			Height = height;
			Name = name;
		}
	}
}
