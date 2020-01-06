using System;
using OpenTK.Graphics.OpenGL;

namespace Rs317.Sharp
{
	public class TextureManager
	{
		private static float PERC_64 = 1f / 64f;
		private static float PERC_128 = 1f / 128f;

		private static int TEXTURE_SIZE = 128;

		public int initTextureArray(ITextureProvider textureProvider)
		{
			if(!allTexturesLoaded(textureProvider))
			{
				return -1;
			}

			ITexture[] textures = textureProvider.Textures;

			int textureArrayId = GLUtil.glGenTexture();
			GL.BindTexture(TextureTarget.Texture2DArray, textureArrayId);
			GL.TexStorage3D(TextureTarget3d.Texture2DArray, 1, SizedInternalFormat.Rgba8, TEXTURE_SIZE, TEXTURE_SIZE, textures.Length);

			GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);

			GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);

			// Set brightness to 1.0d to upload unmodified textures to GPU
			double save = textureProvider.Brightness;
			textureProvider.Brightness = 1.0d;

			updateTextures(textureProvider, textureArrayId);

			textureProvider.Brightness = save;

			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2DArray, textureArrayId);
			GL.ActiveTexture(TextureUnit.Texture0);

			return textureArrayId;
		}

		public void freeTextureArray(int textureArrayId)
		{
			GLUtil.glDeleteTexture(textureArrayId);
		}

		/**
		 * Check if all textures have been loaded and cached yet.
		 *
		 * @param textureProvider
		 * @return
		 */
		private static bool allTexturesLoaded(ITextureProvider textureProvider)
		{
			ITexture[] textures = textureProvider.Textures;
			if(textures == null || textures.Length == 0)
			{
				return false;
			}

			for(int textureId = 0; textureId < textures.Length; textureId++)
			{
				ITexture texture = textures[textureId];
				if(texture != null)
				{
					int[] pixels = textureProvider.GetTexturePixels(textureId);
					if(pixels == null)
					{
						return false;
					}
				}
			}

			return true;
		}

		private static void updateTextures(ITextureProvider textureProvider, int textureArrayId)
		{
			ITexture[] textures = textureProvider.Textures;

			GL.BindTexture(TextureTarget.Texture2DArray, textureArrayId);

			int cnt = 0;
			for(int textureId = 0; textureId < textures.Length; textureId++)
			{
				ITexture texture = textures[textureId];
				if(texture != null)
				{
					int[] srcPixels = textureProvider.GetTexturePixels(textureId);
					if(srcPixels == null)
					{
						Console.WriteLine($"No pixels for texture {textureId}!");
						continue; // this can't happen
					}

					++cnt;

					if(srcPixels.Length != TEXTURE_SIZE * TEXTURE_SIZE)
					{
						// The texture storage is 128x128 bytes, and will only work correctly with the
						// 128x128 textures from high detail mode
						Console.WriteLine($"Texture size for {textureId} is {srcPixels.Length}!");
						continue;
					}


					//ByteBuffer pixelBuffer = ByteBuffer.wrap(pixels);
					//gl.glTexSubImage3D(gl.GL_TEXTURE_2D_ARRAY, 0, 0, 0, textureId, TEXTURE_SIZE, TEXTURE_SIZE,
					//	1, gl.GL_RGBA, gl.GL_UNSIGNED_BYTE, pixelBuffer);

					byte[] pixels = convertPixels(srcPixels, TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE, TEXTURE_SIZE);
					GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, textureId, TEXTURE_SIZE, TEXTURE_SIZE, 1, PixelFormat.Rgba, PixelType.UnsignedByte, pixels);
				}
			}

			Console.WriteLine($"Uploaded textures {cnt}");
		}

		private static byte[] convertPixels(int[] srcPixels, int width, int height, int textureWidth, int textureHeight)
		{
			byte[] pixels = new byte[textureWidth * textureHeight * 4];

			int pixelIdx = 0;
			int srcPixelIdx = 0;

			int offset = (textureWidth - width) * 4;

			for(int y = 0; y < height; y++)
			{
				for(int x = 0; x < width; x++)
				{
					int rgb = srcPixels[srcPixelIdx++];
					if(rgb != 0)
					{
						pixels[pixelIdx++] = (byte)(rgb >> 16);
						pixels[pixelIdx++] = (byte)(rgb >> 8);
						pixels[pixelIdx++] = (byte)rgb;
						pixels[pixelIdx++] = (byte)255;
					}
					else
					{
						pixelIdx += 4;
					}
				}
				pixelIdx += offset;
			}
			return pixels;
		}

		/**
		 * Animate the given texture
		 *
		 * @param texture
		 * @param diff    Number of elapsed client ticks since last animation
		 */
		public void animate(ITexture texture, int diff)
		{
			int[] pixels = texture.Pixels;
			if(pixels == null)
			{
				return;
			}

			int animationSpeed = texture.AnimationSpeed;
			float uvdiff = pixels.Length == 4096 ? PERC_64 : PERC_128;

			float u = texture.UVCoordU;
			float v = texture.UVCoordV;

			int offset = animationSpeed * diff;
			float d = (float)offset * uvdiff;

			switch(texture.AnimationDirection)
			{
				case 1:
					v -= d;
					if(v < 0f)
					{
						v += 1f;
					}
					break;
				case 3:
					v += d;
					if(v > 1f)
					{
						v -= 1f;
					}
					break;
				case 2:
					u -= d;
					if(u < 0f)
					{
						u += 1f;
					}
					break;
				case 4:
					u += d;
					if(u > 1f)
					{
						u -= 1f;
					}
					break;
				default:
					return;
			}

			texture.UVCoordU = u;
			texture.UVCoordV = v;
		}
	}
}
