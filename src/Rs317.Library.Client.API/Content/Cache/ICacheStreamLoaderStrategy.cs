using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	//TODO: make async
	public interface ICacheStreamLoaderStrategy
	{
		/// <summary>
		/// Loads the Cache.dat file as a stream.
		/// </summary>
		/// <returns>A stream containing the cache.dat file.</returns>
		Task<Stream> LoadCacheDatFileAsync();

		/// <summary>
		/// Loads the idx cache file with the provided <see cref="index"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Stream containing the index file.</returns>
		Task<Stream> LoadCacheIndexFileAsync(int index);
	}
}
