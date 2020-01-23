using System;

namespace Rs317.Sharp
{
	public sealed class IndexedImage : DrawingArea
	{
		public byte[] pixels;

		public int[] palette;

		public int width;

		public int height;

		public int drawOffsetX;

		public int drawOffsetY;

		public int resizeWidth;

		private int resizeHeight;

		public bool isValid { get; } = true;

		public IndexedImage(Archive archive, String name, int id, Default317Buffer optionalMetaDataBuffer = null)
		{
			try
			{
				Default317Buffer imageBuffer = new Default317Buffer(archive.decompressFile(name + ".dat"));
				Default317Buffer metadataBuffer = optionalMetaDataBuffer != null ? optionalMetaDataBuffer : new Default317Buffer(archive.decompressFile("index.dat"));

				metadataBuffer.position = imageBuffer.getUnsignedLEShort();
				resizeWidth = metadataBuffer.getUnsignedLEShort();
				resizeHeight = metadataBuffer.getUnsignedLEShort();

				int colourCount = metadataBuffer.getUnsignedByte();
				palette = new int[colourCount];
				for(int c = 0; c < colourCount - 1; c++)
					palette[c + 1] = metadataBuffer.get3Bytes();

				for(int i = 0; i < id; i++)
				{
					metadataBuffer.position += 2;
					imageBuffer.position += metadataBuffer.getUnsignedLEShort() * metadataBuffer.getUnsignedLEShort();
					metadataBuffer.position++;
				}

				drawOffsetX = metadataBuffer.getUnsignedByte();
				drawOffsetY = metadataBuffer.getUnsignedByte();
				width = metadataBuffer.getUnsignedLEShort();
				height = metadataBuffer.getUnsignedLEShort();
				int type = metadataBuffer.getUnsignedByte();
				int pixelCount = width * height;

				//Custom: Below are some sanity checks that are custom but help guard against known clean cache data issues.
				bool isEnoughDataAvailable = pixelCount <= (imageBuffer.buffer.Length - imageBuffer.position);

				//Don't let corrupt image data, in default cache, cause BIG allocation (bad for WebGL)
				//or allocate/read for empty images.
				if(pixelCount <= 0 || pixelCount > int.MaxValue / 100 || !isEnoughDataAvailable || imageBuffer.position < 0) //sometimes happens!!
				{
					width = 0;
					height = 0;
					this.pixels = Array.Empty<byte>();
					return;
				}

				pixels = new byte[pixelCount];

				if(type == 0)
				{
					for(int i = 0; i < pixelCount; i++)
					{
						pixels[i] = imageBuffer.get();
					}

					return;
				}

				if(type == 1)
				{
					for(int x = 0; x < width; x++)
					{
						for(int y = 0; y < height; y++)
						{
							pixels[x + y * width] = imageBuffer.get();
						}
					}
				}
			}
			catch (Exception e)
			{
				isValid = false;

				//Don't throw, this is just a data error. Not an engine fault.
				throw new InvalidOperationException($"Failed to generate IndexedImage for: {name} id: {id}. Reason: {e.Message}\nStack: {e.StackTrace}", e);
			}
		}

		public void draw(int x, int y)
		{
			if (!isValid)
				return;

			x += drawOffsetX;
			y += drawOffsetY;
			int l = x + y * DrawingArea.width;
			int i1 = 0;
			int localHeight = this.height;
			int localWidth = this.width;
			int l1 = DrawingArea.width - localWidth;
			int i2 = 0;
			if(y < DrawingArea.topY)
			{
				int j2 = DrawingArea.topY - y;
				localHeight -= j2;
				y = DrawingArea.topY;
				i1 += j2 * localWidth;
				l += j2 * DrawingArea.width;
			}

			if(y + localHeight > DrawingArea.bottomY)
				localHeight -= (y + localHeight) - DrawingArea.bottomY;
			if(x < DrawingArea.topX)
			{
				int k2 = DrawingArea.topX - x;
				localWidth -= k2;
				x = DrawingArea.topX;
				i1 += k2;
				l += k2;
				i2 += k2;
				l1 += k2;
			}

			if(x + localWidth > DrawingArea.bottomX)
			{
				int l2 = (x + localWidth) - DrawingArea.bottomX;
				localWidth -= l2;
				i2 += l2;
				l1 += l2;
			}

			if(!(localWidth <= 0 || localHeight <= 0))
			{
				draw(localHeight, DrawingArea.pixels, pixels, l1, l, localWidth, i1, palette, i2);
			}
		}

		public void resizeToHalf()
		{
			if(!isValid)
				return;

			resizeWidth /= 2;
			resizeHeight /= 2;

			byte[] tempPixels = new byte[resizeWidth * resizeHeight];
			int i = 0;
			for(int x = 0; x < height; x++)
			{
				for(int y = 0; y < width; y++)
				{
					tempPixels[(y + drawOffsetX >> 1) + (x + drawOffsetY >> 1) * resizeWidth] = pixels[i++];
				}
			}

			this.pixels = tempPixels;
			width = resizeWidth;
			height = resizeHeight;
			drawOffsetX = 0;
			drawOffsetY = 0;
		}

		public void resize()
		{
			if(!isValid)
				return;

			if(width == resizeWidth && height == resizeHeight)
				return;

			byte[] tempPixels = new byte[resizeWidth * resizeHeight];
			int i = 0;
			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					tempPixels[x + drawOffsetX + (y + drawOffsetY) * resizeWidth] = pixels[i++];
				}
			}

			this.pixels = tempPixels;
			width = resizeWidth;
			height = resizeHeight;
			drawOffsetX = 0;
			drawOffsetY = 0;
		}

		public void flipHorizontally()
		{
			if(!isValid)
				return;

			byte[] tempPixels = new byte[width * height];
			int i = 0;
			for(int y = 0; y < height; y++)
			{
				for(int x = width - 1; x >= 0; x--)
				{
					tempPixels[i++] = pixels[x + y * width]; //This is broken on 317Refactor. On stock 317Refactor it actually assigns to itself.
				}
			}

			this.pixels = tempPixels;
			drawOffsetX = resizeWidth - width - drawOffsetX;
		}

		public void flipVertically()
		{
			if(!isValid)
				return;

			byte[] tempPixels = new byte[width * height];
			int i = 0;
			for(int y = height - 1; y >= 0; y--)
			{
				for(int x = 0; x < width; x++)
				{
					tempPixels[i++] = pixels[x + y * width]; //This is broken on 317Refactor. On stock 317Refactor it actually assigns to itself.
				}
			}

			this.pixels = tempPixels;
			drawOffsetY = resizeHeight - height - drawOffsetY;
		}

		private void draw(int i, int[] pixelsToDraw, byte[] image, int j, int k, int l, int i1, int[] paletteToDraw, int j1)
		{
			if(!isValid)
				return;

			int k1 = -(l >> 2);
			l = -(l & 3);
			for(int l1 = -i; l1 < 0; l1++)
			{
				for(int i2 = k1; i2 < 0; i2++)
				{
					byte pixel = image[i1++];
					if(pixel != 0)
						pixelsToDraw[k++] = paletteToDraw[pixel & 0xff];
					else
						k++;
					pixel = image[i1++];
					if(pixel != 0)
						pixelsToDraw[k++] = paletteToDraw[pixel & 0xff];
					else
						k++;
					pixel = image[i1++];
					if(pixel != 0)
						pixelsToDraw[k++] = paletteToDraw[pixel & 0xff];
					else
						k++;
					pixel = image[i1++];
					if(pixel != 0)
						pixelsToDraw[k++] = paletteToDraw[pixel & 0xff];
					else
						k++;
				}

				for(int j2 = l; j2 < 0; j2++)
				{
					byte pixel = image[i1++];
					if(pixel != 0)
						pixelsToDraw[k++] = paletteToDraw[pixel & 0xff];
					else
						k++;
				}

				k += j;
				i1 += j1;
			}

		}

		public void mixPalette(int red, int green, int blue)
		{
			if(!isValid)
				return;

			for(int i = 0; i < palette.Length; i++)
			{
				int r = palette[i] >> 16 & 0xff;
				r += red;
				if(r < 0)
					r = 0;
				else if(r > 255)
					r = 255;
				int g = palette[i] >> 8 & 0xff;
				g += green;
				if(g < 0)
					g = 0;
				else if(g > 255)
					g = 255;
				int b = palette[i] & 0xff;
				b += blue;
				if(b < 0)
					b = 0;
				else if(b > 255)
					b = 255;
				palette[i] = (r << 16) + (g << 8) + b;
			}
		}
	}
}
