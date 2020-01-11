using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	//Default implementation based on the original RSClient handling.
	public sealed class FileDirectoryBasedCacheStreamLoader : ICacheStreamLoaderStrategy
	{
		private string Directory { get; }

		public FileDirectoryBasedCacheStreamLoader(string directory)
		{
			Directory = directory ?? throw new ArgumentNullException(nameof(directory));
		}

		public Task<Stream> LoadCacheDatFileAsync()
		{
			//Logic from old Signlik run
			return Task.FromResult((Stream)new FileStream(Path.Combine(Directory, "main_file_cache.dat"), FileMode.Open));
		}

		public Task<Stream> LoadCacheIndexFileAsync(int index)
		{
			return Task.FromResult((Stream)new FileStream(Path.Combine(Directory, $"main_file_cache.idx{index}"), FileMode.Open));
		}
	}
}
