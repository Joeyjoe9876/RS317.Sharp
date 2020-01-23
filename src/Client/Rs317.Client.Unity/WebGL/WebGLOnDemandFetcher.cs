using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class WebGLOnDemandFetcher : OnDemandFetcher
	{
		public WebGLOnDemandFetcher([NotNull] ITaskDelayFactory taskDelayFactory)
			: base(taskDelayFactory)
		{

		}

		public async Task preloadRegionsAsync(bool flag)
		{
			int j = MapIndices.Count;
			for(int k = 0; k < j; k++)
				if(flag || MapIndices[k].isMembers)
				{
					setPriority((byte)2, 3, MapIndices[k].ObjectFileId);
					setPriority((byte)2, 3, MapIndices[k].TerrainId);

					if(k % 2 == 0)
						await TaskDelayFactory.Create(1);
				}
		}
	}
}
