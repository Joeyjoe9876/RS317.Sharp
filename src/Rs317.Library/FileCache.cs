
using System;
using System.IO;
using System.Runtime.CompilerServices;

/// <summary>
/// Represents a file cache containing multiple archives.
/// </summary>
public sealed class FileCache
{
	private static byte[] buffer = new byte[520];
	private FileStream dataFile;
	private FileStream indexFile;
	private int storeId;

	public FileCache(FileStream data, FileStream index, int storeId)
	{
		this.storeId = storeId;
		this.dataFile = data;
		this.indexFile = index;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public byte[] decompress(int index)
	{
		try
		{
			seek(indexFile, index * 6);
			int inValue;
			for(int r = 0; r < 6; r += inValue)
			{
				inValue = indexFile.Read(buffer, r, 6 - r);
				if(inValue == -1)
					return null;
			}

			int size = ((buffer[0] & 0xff) << 16) + ((buffer[1] & 0xff) << 8) + (buffer[2] & 0xff);
			int sector = ((buffer[3] & 0xff) << 16) + ((buffer[4] & 0xff) << 8) + (buffer[5] & 0xff);

			if(size < 0 || size > 0x7a120)
				return null;

			if(sector <= 0 || sector > dataFile.Length / 520L)
				return null;

			byte[] decompressed = new byte[size];
			int read = 0;
			for(int part = 0; read < size; part++)
			{
				if(sector == 0)
					return null;
				seek(dataFile, sector * 520);
				int r = 0;
				int unread = size - read;
				if(unread > 512)
					unread = 512;
				int in_;
				for(; r < unread + 8; r += in_)
				{
					in_ = dataFile.Read(buffer, r, (unread + 8) - r);
					if(in_ == -1)
						return null;
				}

				int decompressedIndex = ((buffer[0] & 0xff) << 8) + (buffer[1] & 0xff);
				int decompressedPart = ((buffer[2] & 0xff) << 8) + (buffer[3] & 0xff);
				int decompressedSector = ((buffer[4] & 0xff) << 16) + ((buffer[5] & 0xff) << 8) + (buffer[6] & 0xff);
				int decompressedStoreId = buffer[7] & 0xff;

				if(decompressedIndex != index || decompressedPart != part || decompressedStoreId != storeId)
					return null;

				if(decompressedSector < 0 || decompressedSector > dataFile.Length / 520L)
					return null;

				for(int i = 0; i < unread; i++)
					decompressed[read++] = buffer[i + 8];

				sector = decompressedSector;
			}

			return decompressed;
		}
		catch(IOException _ex)
		{
			return null;
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public bool put(int size, byte[] data, int index)
	{
		bool exists = put(true, index, size, data);
		if(!exists)
			exists = put(false, index, size, data);
		return exists;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	private bool put(bool exists, int index, int size, byte[] data)
	{
		try
		{
			int sector;
			if(exists)
			{
				seek(indexFile, index * 6);
				int inValue;
				for(int r = 0; r < 6; r += inValue)
				{
					inValue = indexFile.Read(buffer, r, 6 - r);
					if(inValue == -1)
						return false;
				}

				sector = ((buffer[3] & 0xff) << 16) + ((buffer[4] & 0xff) << 8) + (buffer[5] & 0xff);
				if(sector <= 0 || sector > dataFile.Length / 520L)
					return false;
			}
			else
			{
				sector = (int)((dataFile.Length + 519L) / 520L);
				if(sector == 0)
					sector = 1;
			}
			buffer[0] = (byte)(size >> 16);
			buffer[1] = (byte)(size >> 8);
			buffer[2] = (byte)size;
			buffer[3] = (byte)(sector >> 16);
			buffer[4] = (byte)(sector >> 8);
			buffer[5] = (byte)sector;
			seek(indexFile, index * 6);
			indexFile.Write(buffer, 0, 6);
			int written = 0;
			for(int part = 0; written < size; part++)
			{
				int decompressedSector = 0;
				if(exists)
				{
					seek(dataFile, sector * 520);
					int read;
					int inValue;
					for(read = 0; read < 8; read += inValue)
					{
						inValue = dataFile.Read(buffer, read, 8 - read);
						if(inValue == -1)
							break;
					}

					if(read == 8)
					{
						int decompressedIndex = ((buffer[0] & 0xff) << 8) + (buffer[1] & 0xff);
						int decompressedPart = ((buffer[2] & 0xff) << 8) + (buffer[3] & 0xff);
						decompressedSector = ((buffer[4] & 0xff) << 16) + ((buffer[5] & 0xff) << 8)
													+ (buffer[6] & 0xff);
						int decompressedStoreId = buffer[7] & 0xff;
						if(decompressedIndex != index || decompressedPart != part || decompressedStoreId != storeId)
							return false;
						if(decompressedSector < 0 || decompressedSector > dataFile.Length / 520L)
							return false;
					}
				}

				if(decompressedSector == 0)
				{
					exists = false;
					decompressedSector = (int)((dataFile.Length + 519L) / 520L);
					if(decompressedSector == 0)
						decompressedSector++;
					if(decompressedSector == sector)
						decompressedSector++;
				}

				if(size - written <= 512)
					decompressedSector = 0;

				buffer[0] = (byte)(index >> 8);
				buffer[1] = (byte)index;
				buffer[2] = (byte)(part >> 8);
				buffer[3] = (byte)part;
				buffer[4] = (byte)(decompressedSector >> 16);
				buffer[5] = (byte)(decompressedSector >> 8);
				buffer[6] = (byte)decompressedSector;
				buffer[7] = (byte)storeId;
				seek(dataFile, sector * 520);
				dataFile.Write(buffer, 0, 8);

				int unwritten = size - written;
				if(unwritten > 512)
					unwritten = 512;
				dataFile.Write(data, written, unwritten);
				written += unwritten;
				sector = decompressedSector;
			}
			return true;
		}
		catch(IOException _ex)
		{
			return false;
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	private void seek(FileStream file, int position)
	{
		if (file == null) throw new ArgumentNullException(nameof(file));

		if(position < 0 || position > 0x3c00000)
			throw new InvalidOperationException($"Badseek - pos:{position} len:{file.Length}");

		file.Seek(position, SeekOrigin.Begin);
	}
}