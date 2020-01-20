using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Rs317.Sharp
{
	public sealed class RsUnityPS4Client : RsUnityClient
	{
		public RsUnityPS4Client(ClientConfiguration config, UnityRsGraphics graphicsObject) 
			: base(config, graphicsObject, new DefaultRunnableStarterStrategy())
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			Sprite.ExternalLoadImageHook += ExternalLoadImageHook;
		}

		private LoadedImagePixels ExternalLoadImageHook(byte[] arg)
		{
			//LoadImage will replace with with incoming image size.
			Texture2D tex = new Texture2D(2, 2, TextureFormat.RGB24, false);
			ImageConversion.LoadImage(tex, arg);
			tex = FlipTexture(tex);
			int[] pixels = tex.GetPixels32().Select(ColorUtils.ColorToRGB).ToArray();

			return new LoadedImagePixels(tex.height, tex.width, pixels);
		}

		Texture2D FlipTexture(Texture2D original)
		{
			Texture2D flipped = new Texture2D(original.width, original.height);

			int xN = original.width;
			int yN = original.height;

			for(int i = 0; i < xN; i++)
			{
				for(int j = 0; j < yN; j++)
				{
					flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
				}
			}
			flipped.Apply();

			return flipped;
		}
	}
}
