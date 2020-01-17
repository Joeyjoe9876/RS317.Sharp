using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	//Based on: https://raw.githubusercontent.com/apollo-rsps/apollo/90659291ea6c5d96128eab410f1261a89c4d2fb8/cache/src/main/java/org/apollo/cache/map/MapConstants.java
	public static class MapConstants
	{
		/// <summary>
		/// The index containing the map files.
		/// </summary>
		public const int MAP_INDEX = 4;

		/// <summary>
		/// The width (and length) of a {@link MapFile} in {@link Tile}s.
		/// </summary>
		public const int MAP_WIDTH = 64;

		/// <summary>
		/// The amount of planes in a MapFile.
		/// </summary>
		public const int MAP_PLANES = 4;

		/// <summary>
		/// The multiplicand for height values.
		/// </summary>
		public const int HEIGHT_MULTIPLICAND = 8;

		/// <summary>
		/// The lowest type value that will result in the decoding of a Tile being continued.
		/// </summary>
		public const int LOWEST_CONTINUED_TYPE = 2;

		/// <summary>
		/// The minimum type that specifies the Tile attributes.
		/// </summary>
		public const int MINIMUM_ATTRIBUTES_TYPE = 81;

		/// <summary>
		/// The minimum type that specifies the Tile underlay id.
		/// </summary>
		public const int MINIMUM_OVERLAY_TYPE = 49;

		/// <summary>
		/// The amount of possible overlay orientations.
		/// </summary>
		public const int ORIENTATION_COUNT = 4;

		/// <summary>
		/// The height difference between two planes.
		/// </summary>
		public const int PLANE_HEIGHT_DIFFERENCE = 240;
	}
}
