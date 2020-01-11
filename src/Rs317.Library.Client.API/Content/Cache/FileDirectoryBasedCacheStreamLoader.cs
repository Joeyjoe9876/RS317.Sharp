using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rs317.Sharp
{
	//Default implementation based on the original RSClient handling.
	public sealed class FileDirectoryBasedCacheStreamLoader : ICacheStreamLoader
	{
		private string Directory { get; }

		public FileDirectoryBasedCacheStreamLoader(string directory)
		{
			Directory = directory ?? throw new ArgumentNullException(nameof(directory));
		}

		public Stream LoadCacheDatFile()
		{
			//Logic from old Signlik run
			return new FileStream(Path.Combine(Directory, "main_file_cache.dat"), FileMode.Open);
		}

		public Stream LoadCacheIndexFile(int index)
		{
			return new FileStream(Path.Combine(Directory, $"main_file_cache.idx{index}"), FileMode.Open);
		}
	}
}
