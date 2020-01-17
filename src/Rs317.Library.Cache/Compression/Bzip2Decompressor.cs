
//From Major at Rune-Server
//https://www.rune-server.ee/runescape-development/rs2-client/snippets/430099-bzip2-classes-refactor.html

using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace Rs317.Sharp
{
	/// <summary>
	/// http://svn.apache.org/repos/asf/labs/axmake/trunk/src/libuc++/srclib/bzip2/
	/// </summary>
	public sealed class Bzip2Decompressor
	{
		//Jagex uses headerless BZip2 because they're INSANE like other game dev studios.
		public static byte[] BZIP2_HEADER = { 66, 90, 104, 49 };

		//Renamed retVal to inputLength
		public static int decompress(byte[] output, int expectedOutputSize, byte[] input, int maxLen, int inputOffset)
		{
			//Have to include BZIP2 Header
			//using (MemoryStream inputStream = new MemoryStream(input, inputOffset, input.Length - inputOffset, false))
			using(MemoryStream inputStream = new MemoryStream(input.Length - inputOffset + 4))
			{
				inputStream.Write(BZIP2_HEADER, 0, 4);
				inputStream.Write(input, inputOffset, input.Length - inputOffset);
				inputStream.Position = 0;
				using(MemoryStream outputStream = new MemoryStream(output, 0, expectedOutputSize, true))
				{
					BZip2.Decompress(inputStream, outputStream, true);
				}
			}

			//TODO: Does Jagex expect us to suppress errors??
			return expectedOutputSize;
		}
	}
}