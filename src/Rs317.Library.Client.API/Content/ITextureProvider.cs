using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface ITextureProvider
	{
		/**
		 * Set the brightness for textures, clearing the texture cache.
		 *
		 * .9 is the darkest value available in the standard options
		 * .6 is the brightest value
		 */
		double Brightness { get; set; }

		/**
		 * Get all textures
		 */
		ITexture[] Textures { get; }

		/**
		 * Get the pixels for a texture
		 */
		int[] GetTexturePixels(int textureId);
	}
}
