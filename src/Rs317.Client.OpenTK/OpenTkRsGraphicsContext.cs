using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Rs317.Sharp
{
	public sealed class OpenTKRsGraphicsContext
	{
		public void DrawImage(Bitmap image, int x, int y, int width, int height)
		{
			//TODO: This is a hacky lock, we need a better way to render.
			lock(OpenTKGameWindow.DrawImageQueue)
				OpenTKGameWindow.DrawImageQueue.Enqueue(new DrawImageQueueable(x, y, image, width, height));
		}
	}
}
