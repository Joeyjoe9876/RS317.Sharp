
//From Major at Rune-Server
//https://www.rune-server.ee/runescape-development/rs2-client/snippets/430099-bzip2-classes-refactor.html

using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

/// <summary>
/// http://svn.apache.org/repos/asf/labs/axmake/trunk/src/libuc++/srclib/bzip2/
/// </summary>
public sealed class Bzip2Decompressor
{
	//Renamed retVal to inputLength
	public static int decompress(byte[] output, int expectedOutputSize, byte[] input, int maxLen, int inputOffset)
	{
		using (MemoryStream inputStream = new MemoryStream(input, inputOffset, input.Length - inputOffset, false))
		{
			using (MemoryStream outputStream = new MemoryStream(output, 0, maxLen, true))
			{
				BZip2.Decompress(inputStream, outputStream, true);
			}
		}

		//TODO: Does Jagex expect us to suppress errors??
		return expectedOutputSize;
	}
}