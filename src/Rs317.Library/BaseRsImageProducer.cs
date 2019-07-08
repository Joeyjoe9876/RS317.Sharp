using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Rs317.Sharp
{
	public abstract class BaseRsImageProducer<TGraphicsType>
	{
		public virtual int[] pixels { get; }

		public int width { get; }

		public int height { get; }

		protected BaseRsImageProducer(int width, int height)
		{
			this.width = width;
			this.height = height;
			pixels = new int[width * height];
			initDrawingArea();
		}

		public void initDrawingArea()
		{
			DrawingArea.initDrawingArea(height, width, pixels);
		}

		/// <summary>
		/// Publicly exposed graphics drawing method found in the original Java client.
		/// </summary>
		/// <param name="y"></param>
		/// <param name="g"></param>
		/// <param name="x"></param>
		public void drawGraphics(int y, IRSGraphicsProvider<TGraphicsType> g, int x)
		{
			OnBeforeInternalDrawGraphics(y, x);
			lock (g.SyncObj)
			{
				InternalDrawGraphics(x, y, g);
			}
		}

		/// <summary>
		/// Called BEFORE the graphics drawing is requested.
		/// Do setup/initialization work here because the lock on the graphics object isn't aquiried here yet.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		protected abstract void OnBeforeInternalDrawGraphics(int x, int z);

		/// <summary>
		/// Called when the graphics drawing is requested.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="graphicsObject"></param>
		protected abstract void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<TGraphicsType> graphicsObject);
	}
}
