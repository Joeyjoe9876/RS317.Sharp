using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class TextureDrawable
	{
		private Texture2D Texture { get; }

		/// <summary>
		/// Can be set to move the image.
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Can be set to move the image.
		/// </summary>
		public int Y { get; set; }

		public TextureDrawable([NotNull] Texture2D texture)
		{
			Texture = texture ?? throw new ArgumentNullException(nameof(texture));
		}

		public void Draw()
		{
			Graphics.DrawTexture(CalculateScreenRect(Texture, X, Y), Texture);
		}

		private Rect CalculateScreenRect(Texture2D image, int x, int y)
		{
			/*
			float xOffset = (float) renderable.ImageLocation.X;
			float yOffset = (float) renderable.ImageLocation.Y;

			//Scaling code for resizable
			//765, 503 default size.

			//Get current size modifier
			float widthModifier = (float)this.Width / 765.0f;
			float heightModifier = (float)this.Height / 503.0f;

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(xOffset * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 0); GL.Vertex2((renderable.ImageLocation.Width + xOffset) * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 1); GL.Vertex2((renderable.ImageLocation.Width + xOffset) * widthModifier, (renderable.ImageLocation.Height + yOffset) * heightModifier);
			GL.TexCoord2(0, 1); GL.Vertex2(xOffset * widthModifier, (renderable.ImageLocation.Height + yOffset) * heightModifier);
			GL.End();
			*/

			float xOffset = (float)X;
			float yOffset = (float)Y;

			float widthModifier = (float)Screen.width / 765.0f;
			float heightModifier = (float)Screen.height / 503.0f;

			return new Rect(xOffset * widthModifier, 503.0f - yOffset - image.height, image.width * widthModifier, image.height * heightModifier);
			//return new Rect(xOffset * widthModifier, yOffset * heightModifier, image.width * widthModifier, image.height * heightModifier);
		}
	}
}
