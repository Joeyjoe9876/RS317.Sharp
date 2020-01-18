using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Rs317.Sharp
{
	//Based loosely on: https://github.com/apollo-rsps/apollo/blob/kotlin-experiments/cache/src/main/java/org/apollo/cache/map/MapObjectsDecoder.java
	public sealed class MapObjectsListArchiveDeserializer
	{
		private Default317Buffer MapObjectListBuffer { get; }

		public MapObjectsListArchiveDeserializer(FileCache fileCache, MapIndex index)
		{
			byte[] bytes = fileCache.decompress(index.ObjectFileId);
			
			using (GZipStream gzipinputstream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
			{
				using(MemoryStream stream = new MemoryStream())
				{
					byte[] buffer = new byte[1024];
					do
					{
						int k = gzipinputstream.Read(buffer, 0, buffer.Length);
						if(k == -1 || k == 0) //This was causing a hang here, different return value in C#. RS2Sharp made this change, but did not document it.
							break;

						//Write the bytes to the stream.
						stream.Write(buffer, 0, k);
					} while(true);

					bytes = stream.ToArray();
				}
			}

			MapObjectListBuffer = new Default317Buffer(bytes);
		}

		public IReadOnlyList<MapObject> Deserialize()
		{
			List<MapObject> mapObjects = new List<MapObject>(20);

			//Following code is from Region.loadObjectBlock
			Default317Buffer stream = MapObjectListBuffer;
			int objectId = -1;
			do
			{
				int objectIdOffset = stream.getSmartB();
				if(objectIdOffset == 0)
					break;
				objectId += objectIdOffset;
				int position = 0;
				do
				{
					int positionOffset = stream.getSmartB();
					if(positionOffset == 0)
						break;
					position += positionOffset - 1;

					int hash = stream.getUnsignedByte();
					int type = hash >> 2;
					int orientation = hash & 3;

					mapObjects.Add(new MapObject(objectId, position, type, orientation));
				} while(true);
			} while(true);

			return mapObjects;
		}
	}
}
