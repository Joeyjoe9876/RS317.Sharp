using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class UnityRsImageProducer : BaseRsImageProducer<UnityRsGraphics>
	{
		private Task<TextureCreationResult> Image { get; }

		//Temp buff used for efficient copy to NativeArray
		//Unity3D native array set is SLOW.
		private int[] TempBufferPixels { get; }

		public UnityRsImageProducer(int width, int height, string name, IRSGraphicsProvider<UnityRsGraphics> graphicsProvider)
			: base(width, height, name)
		{
			try
			{
				lock (graphicsProvider.SyncObj)
				{
					Image = graphicsProvider.GameGraphics.CreateTexture(new TextureCreationRequest(width, height, name));
					TempBufferPixels = new int[width * height];
				}
			}
			catch (Exception e)
			{
				throw new InvalidOperationException($"Failed to create ImageProducer for: {name}", e);
				throw;
			}

			initDrawingArea();
		}

		protected override void OnBeforeInternalDrawGraphics(int x, int z, IRSGraphicsProvider<UnityRsGraphics> graphicsObject)
		{
			//Texture creation is async
			if (!Image.IsCompleted)
				return;

			method239();
		}

		protected override void InternalDrawGraphics(int x, int y, IRSGraphicsProvider<UnityRsGraphics> rsGraphicsProvider, bool force = false)
		{
			//Might not be created yet, not promised to be created.
			if(!Image.IsCompleted)
				return;

			lock(rsGraphicsProvider.SyncObj)
			{
				rsGraphicsProvider.GameGraphics.DrawImageToScreen(this.Name, x, y);
			}
		}

		private unsafe void method239()
		{
			NativeArray<int> ptr = Image.Result.NativePtr;

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int index = (x + y * width);
					int value = base.pixels[index];

					//The 255 << 24 is the alpha bits that must be set.
					//ptr[index] = value + (255 << 24);
					TempBufferPixels[index] = value + (255 << 24);
				}
			}

			//Doing temp buffer and copy yielded 33% performance gain. Important for WebGL because main thread is doing this writing.
			//This should be faster than making calls to Unit3D's much slower SET API.
			//Profiled in the editor.
			NativeArray<int>.Copy(TempBufferPixels, ptr);
		}
	}
}
