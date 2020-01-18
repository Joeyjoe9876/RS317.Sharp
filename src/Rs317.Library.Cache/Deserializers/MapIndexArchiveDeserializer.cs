using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public sealed class MapIndexArchiveDeserializer
	{
		public Archive Archive { get; }
		/*
		abyte2 = streamLoader.decompressFile("map_index");
		Default317Buffer stream2 = new Default317Buffer(abyte2);
		j1 = abyte2.Length / 7;
		mapIndices1 = new int[j1];
		mapIndices2 = new int[j1];
		mapIndices3 = new int[j1];
		mapIndices4 = new int[j1];
		for(int i2 = 0; i2 < j1; i2++)
		{
			mapIndices1[i2] = stream2.getUnsignedLEShort();
			mapIndices2[i2] = stream2.getUnsignedLEShort();
			mapIndices3[i2] = stream2.getUnsignedLEShort();
			mapIndices4[i2] = stream2.getUnsignedByte();
		}
		*/

		public MapIndexArchiveDeserializer(Archive archive)
		{
			Archive = archive ?? throw new ArgumentNullException(nameof(archive));
		}

		private IBufferReadable GenerateMapIndexBuffer()
		{
			byte[] bytes = Archive.decompressFile("map_index");
			return new Default317Buffer(bytes);
		}

		public IReadOnlyList<MapIndex> Deserialize()
		{
			IBufferReadable buffer = GenerateMapIndexBuffer();
			int mapIndexCount = buffer.buffer.Length / (sizeof(short) * 3 + sizeof(byte));
			MapIndex[] indexArray = new MapIndex[mapIndexCount];

			for(int index = 0; index < mapIndexCount; index++)
			{
				//ID is basically the unique value represneing bitwise combined X,Y region id.
				int id = buffer.getUnsignedLEShort();
				int terrain = buffer.getUnsignedLEShort();
				int objects = buffer.getUnsignedLEShort();
				bool members = buffer.getUnsignedByte() == 1;

				indexArray[index] = new MapIndex(members, objects, id, terrain);
			}

			return indexArray;
		}
	}
}
