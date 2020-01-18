using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class MapIndex
	{
		/// <summary>
		/// Indicates whether or not this map is members-only.
		/// </summary>
		public bool isMembers { get; }

		/// <summary>
		/// The object file id.
		/// </summary>
		public int ObjectFileId { get; }

		/// <summary>
		/// The packed coordinates.
		/// </summary>
		public int PackedCoordinates { get; }

		/// <summary>
		/// The terrain file id.
		/// </summary>
		public int TerrainId { get; }

		public int X => (PackedCoordinates >> 8 & 0xFF) * MapConstants.MAP_WIDTH;

		public int Y => (PackedCoordinates & 0xFF) * MapConstants.MAP_WIDTH;

		public MapIndex(bool isMembers, int objectFileId, int packedCoordinates, int terrainId)
		{
			this.isMembers = isMembers;
			ObjectFileId = objectFileId;
			PackedCoordinates = packedCoordinates;
			TerrainId = terrainId;
		}
	}
}
