using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;

namespace Rs317.Sharp
{
	public sealed class RsOpenTkGraphicsMode : GraphicsMode
	{
		public RsOpenTkGraphicsMode()
			: base(GraphicsMode.Default.ColorFormat, 24, 8, 0) //Last parameter is FXAA setting. But it doesn't seem to affect anything right now. Must not be screenspace?
		{
			
		}
	}
}
