
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

public sealed class RSImageProducer
{
	public int[] pixels;

	public int width;

	public int height;

	private Bitmap image;

	private FastPixel fastPixel;

	public RSImageProducer(int width, int height, Form component)
	{
		this.width = width;
		this.height = height;
		pixels = new int[width * height];
		image = new Bitmap(width, height);
		fastPixel = new FastPixel(image);
		fastPixel.rgbValues = new byte[width * height * 4];
		initDrawingArea();
	}

	public void initDrawingArea()
	{
		DrawingArea.initDrawingArea(height, width, pixels);
	}

	public void drawGraphics(int y, Graphics g, int x)
	{
		lock (image)
		{
			method239();
			while(true)
			{
				try
				{
					g.DrawImageUnscaled(image, x, y);
					break;
				}
				catch(Exception e)
				{
					//TODO: This is a remanent of the dude who ported this. It shouldn't happen in a good threading model.
					System.Threading.Thread.Sleep(1);
				}
			}
		}
	}

	private void method239()
	{
		fastPixel.Lock();
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				int value = pixels[x + y * width];
				fastPixel.SetPixel(x, y, Color.FromArgb((value >> 16) & 0xFF, (value >> 8) & 0xFF, value & 0xFF));
			}
		}
		fastPixel.Unlock(true);
	}

	public bool imageUpdate(Image image, int i, int j, int k, int l, int i1)
	{
		return true;
	}
}
