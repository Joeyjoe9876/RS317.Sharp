using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class Unity3DResourceCacheLoader : ICacheStreamLoaderStrategy
	{
		//Extension should be .bytes but the resource loader doesn't need it.
		private const string BinaryUnityResourceExtension = "";

		private const string CacheResourceDirectoryPath = "Cache";

		public async Task<Stream> LoadCacheDatFileAsync()
		{
			if(Application.platform != RuntimePlatform.WebGLPlayer)
				await new UnityYieldAwaitable();

			string cachePath = Path.Combine(CacheResourceDirectoryPath, $"main_file_cache.dat{BinaryUnityResourceExtension}");
			TextAsset load = Resources.Load<TextAsset>(cachePath);

			Console.WriteLine($"Cache file loaded. Size: {load?.bytes?.Length}");

			if (load == null || load.bytes == null || load.bytes.Length == 0)
				throw new InvalidOperationException($"Failed to load {cachePath} cache file.");
			else
				return new MemoryStream(load.bytes);
		}

		public async Task<Stream> LoadCacheIndexFileAsync(int index)
		{
			if(Application.platform != RuntimePlatform.WebGLPlayer)
				await new UnityYieldAwaitable();

			string cachePath = Path.Combine(CacheResourceDirectoryPath, $"main_file_cache.idx{index}");
			TextAsset load = Resources.Load<TextAsset>(cachePath);

			Console.WriteLine($"Cache file loaded. Size: {load?.bytes?.Length}");

			if(load == null || load.bytes == null || load.bytes.Length == 0)
				throw new InvalidOperationException($"Failed to load {cachePath} cache file.");
			else
				return new MemoryStream(load.bytes);
		}
	}
}
