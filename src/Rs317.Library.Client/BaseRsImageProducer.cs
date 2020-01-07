using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Rs317.Sharp
{
	public abstract class BaseRsImageProducer<TGraphicsType> : INameable
	{
		public virtual int[] pixels { get; }

		public int width { get; }

		public int height { get; }

		/// <inheritdoc />
		public string Name { get; }

		protected BaseRsImageProducer(int width, int height, string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

			this.width = width;
			this.height = height;
			Name = name;
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
		public void drawGraphics(int y, IRSGraphicsProvider<TGraphicsType> g, int x, bool force = false)
		{
			OnBeforeInternalDrawGraphics(y, x, g);
			lock (g.SyncObj)
			{
				InternalDrawGraphics(x, y, g, force);
			}
		}

		/// <summary>
		/// Called BEFORE the graphics drawing is requested.
		/// Do setup/initialization work here because the lock on the graphics object isn't aquiried here yet.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <param name="graphicsObject"></param>
		protected abstract void OnBeforeInternalDrawGraphics(int x, int z, IRSGraphicsProvider<TGraphicsType> graphicsObject);

		/// <summary>
		/// Called when the graphics drawing is requested.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="graphicsObject"></param>
		protected abstract void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<TGraphicsType> graphicsObject, bool force = false);
	}
}
