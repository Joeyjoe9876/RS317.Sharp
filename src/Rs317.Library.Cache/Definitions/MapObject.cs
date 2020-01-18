using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class MapObject
	{
		/// <summary>
		/// The object definition id of this {@code MapObject}.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// The packed coordinates (local XY and height) for this object.
		/// </summary>
		private int PackedCoordinates { get; }

		/// <summary>
		/// The type of this object.
		/// </summary>
		public int ObjectType { get; }

		/// <summary>
		/// The orientation of this object.
		/// </summary>
		public int Orientation { get; }

		/// <summary>
		/// Get the plane this map object exists on.
		/// </summary>
		public int Plane => PackedCoordinates >> 12 & 0x3;

		/// <summary>
		/// Get the plane this map object exists on.
		/// </summary>
		public int LocalX => PackedCoordinates >> 6 & 0x3F;

		/// <summary>
		/// Get the Y coordinate of this object relative to the map position.
		/// </summary>
		public int LocalY => PackedCoordinates & 0x3F;

		/// <summary>
		/// Creates a new {@code MapObject}.
		/// </summary>
		/// <param name="id">id The object ID of this map object.</param>
		/// <param name="packedCoordinates">packedCoordinates A packed integer containing the coordinates of this map object.</param>
		/// <param name="type">type The type of object.</param>
		/// <param name="orientation">orientation The object facing direction.</param>
		public MapObject(int id, int packedCoordinates, int type, int orientation)
		{
			this.Id = id;
			this.PackedCoordinates = packedCoordinates;
			this.ObjectType = type;
			this.Orientation = orientation;
		}

		/// <summary>
		/// Create a new {@code MapObject}.
		/// </summary>
		/// <param name="id">id The object ID of this map object.</param>
		/// <param name="x">x The local X coordinate of this object.</param>
		/// <param name="y">y The local Y coordinate of this object.</param>
		/// <param name="height">height The height level of this object.</param>
		/// <param name="type">type The type of this object.</param>
		/// <param name="orientation">orientation The orientation of this object.</param>
		public MapObject(int id, int x, int y, int height, int type, int orientation)
			: this(id, (height & 0x3f) << 12 | (x & 0x3f) << 6 | (y & 0x3f), type, orientation)
		{
			
		}
	}
}
