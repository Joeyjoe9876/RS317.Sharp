using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class OpenGlRegisteredOpenTKImageRenderable
	{
		/// <summary>
		/// The renderable image.
		/// </summary>
		public IOpenTKImageRenderable Renderable { get; }

		/// <summary>
		/// The OpenGl texture id.
		/// </summary>
		public int TextureId { get; }

		public OpenGlRegisteredOpenTKImageRenderable(IOpenTKImageRenderable renderable, int textureId)
		{
			if (textureId <= 0) throw new ArgumentOutOfRangeException(nameof(textureId));
			Renderable = renderable ?? throw new ArgumentNullException(nameof(renderable));
			TextureId = textureId;
		}
	}
}
