using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Rs317.Sharp
{
	/// <summary>
	/// The Open GL through OpenTK game window for the game client.
	/// </summary>
	public sealed class OpenTKGameWindow : GameWindow
	{
		public static ConcurrentQueue<DrawImageQueueable> DrawImageQueue { get; } = new ConcurrentQueue<DrawImageQueueable>();

		private Dictionary<Bitmap, int> KnownBitmaps { get; }

		private Dictionary<int, DrawImageQueueable> ImageDrawCommands { get; }

		public OpenTKGameWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, "Rs317.Sharp by Glader")
		{
			KnownBitmaps = new Dictionary<Bitmap, int>();
			ImageDrawCommands = new Dictionary<int, DrawImageQueueable>();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			GL.ClearColor(Color.MidnightBlue);
			GL.Enable(EnableCap.Texture2D);
			GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);

			GL.Viewport(0, 0, Width, Height);

			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref ortho_projection);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
				while (DrawImageQueue.TryDequeue(out var drawRequest))
				{
					lock (drawRequest.Image)
					{
						if (!KnownBitmaps.ContainsKey(drawRequest.Image))
							CreateTexture(drawRequest);
						else
							UpdateTexture(drawRequest);
					}
				}

			foreach (KeyValuePair<int, DrawImageQueueable> imageRequest in ImageDrawCommands)
			{
				BindTexture(imageRequest.Key);
				DrawTexture(imageRequest.Value);
			}
				

			SwapBuffers();

			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		private void BindTexture(int textureId)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureId);
		}

		private static void DrawTexture(DrawImageQueueable drawRequest)
		{
			GL.Begin(PrimitiveType.Quads);

			float xOffset = (float) drawRequest.XDrawOffset;
			float yOffset = (float) drawRequest.XHeightOffset;

			GL.TexCoord2(0, 0);
			GL.Vertex2(xOffset, yOffset);
			GL.TexCoord2(1, 0);
			GL.Vertex2(drawRequest.Width + xOffset, yOffset);
			GL.TexCoord2(1, 1);
			GL.Vertex2(drawRequest.Width + xOffset, drawRequest.Height + yOffset);
			GL.TexCoord2(0, 1);
			GL.Vertex2(xOffset, drawRequest.Height + yOffset);

			GL.End();
		}

		private void UpdateTexture(DrawImageQueueable drawRequest)
		{
			GL.BindTexture(TextureTarget.Texture2D, KnownBitmaps[drawRequest.Image]);
			BitmapData bmpData = drawRequest.Image.LockBits(new Rectangle(0, 0, drawRequest.Image.Width, drawRequest.Image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bmpData.Width, bmpData.Height,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			drawRequest.Image.UnlockBits(bmpData); //Release the bitmap data from memory cause it is not needed anymore.
		}

		private void CreateTexture(DrawImageQueueable drawRequest)
		{
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			int texture;
			GL.GenTextures(1, out texture);
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			KnownBitmaps.Add(drawRequest.Image, texture);
			ImageDrawCommands.Add(texture, drawRequest);

			BitmapData bmpData = drawRequest.Image.LockBits(new Rectangle(0, 0, drawRequest.Image.Width, drawRequest.Image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			drawRequest.Image.UnlockBits(bmpData); //Release the bitmap data from memory cause it is not needed anymore.
		}
	}
}
