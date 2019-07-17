using System;
using System.Drawing;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class UnityTextureImageProducer : BaseRsImageProducer<Graphics>
	{
		private IntPtr ImageDataPointer { get; set; } = (IntPtr)0;

		public UnityTextureImageProducer(int width, int height, string name)
			: base(width, height, name)
		{
			initDrawingArea();
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z)
		{
			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<Graphics> rsGraphicsProvider, bool force = false)
		{
			//TODO: This is hacky, and not high performance to enqueue data like this. Behind the scenes it locks too.
			Unity2DRsRenderer.EnqueueRenderData(new RenderRequestData(Name, new Rect(x, y, width, height), ptr => this.ImageDataPointer = ptr));
		}

		private unsafe void method239()
		{
			//Not initialized yet
			if ((int) ImageDataPointer <= 0)
				return;

			int* pointer = (int*)ImageDataPointer;

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int index = (x + y * width);
					int value = base.pixels[index];

					//The 255 << 24 is the alpha bits that must be set.
					*(pointer + index) = value + (255 << 24);
				}
			}
		}
	}
}
