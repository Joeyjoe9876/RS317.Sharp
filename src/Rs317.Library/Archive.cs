
using System;

/// <summary>
/// Represents a single archive within a cache.
/// </summary>
public sealed class Archive
{
	private byte[] outputData;

	private int fileCount;

	private int[] hashes;
	private int[] decompressedSizes;
	private int[] compressedSizes;
	private int[] initialOffsets;
	private bool decompressed;

	public Archive(byte[] data)
	{
		Buffer buffer = new Buffer(data);
		int compressedLength = buffer.get3Bytes();
		int decompressedLength = buffer.get3Bytes();

		if(decompressedLength != compressedLength)
		{
			byte[] output = new byte[compressedLength];
			Bzip2Decompressor.decompress(output, compressedLength, data, decompressedLength, 6);
			outputData = output;
			buffer = new Buffer(outputData);
			this.decompressed = true;
		}
		else
		{
			outputData = data;
			this.decompressed = false;
		}

		fileCount = buffer.getUnsignedLEShort();
		hashes = new int[fileCount];
		decompressedSizes = new int[fileCount];
		compressedSizes = new int[fileCount];
		initialOffsets = new int[fileCount];
		int offset = buffer.position + fileCount * 10;

		for(int index = 0; index < fileCount; index++)
		{
			hashes[index] = buffer.getInt();
			decompressedSizes[index] = buffer.get3Bytes();
			compressedSizes[index] = buffer.get3Bytes();
			initialOffsets[index] = offset;
			offset += compressedSizes[index];
		}
	}

	public byte[] decompressFile(String name)
	{
		if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

		int hash = 0;
		name = name.ToUpper();
		for(int c = 0; c < name.Length; c++)
			hash = (hash * 61 + name[c]) - 32;

		for(int file = 0; file < fileCount; file++)
			if(hashes[file] == hash)
			{
				byte[] output = new byte[decompressedSizes[file]];
				if(!decompressed)
				{
					Bzip2Decompressor.decompress(output, decompressedSizes[file], outputData, compressedSizes[file],
							initialOffsets[file]);
				}
				else
				{
					System.Buffer.BlockCopy(outputData, initialOffsets[file], output, 0, decompressedSizes[file]);
				}
				return output;
			}

		throw new InvalidOperationException($"Failed to decompress Archive: {name} Expected Hash: {hash}");
	}
}