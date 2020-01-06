using System;

namespace Rs317.Sharp
{
	//Based on: https://raw.githubusercontent.com/runelite/runelite/master/runelite-api/src/main/java/net/runelite/api/Texture.java
	public interface ITexture
	{
		int[] Pixels { get; }

		int AnimationDirection { get; }

		int AnimationSpeed { get; }

		bool isLoaded { get; }

		float UVCoordU { get; set; }

		float UVCoordV { get; set; }
	}
}
