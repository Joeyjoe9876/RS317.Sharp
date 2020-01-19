using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class WebGLMediaLoaderHack : MonoBehaviour
	{
		public RsUnityWebGLClient Client { get; set; }

		private Archive archiveMedia;

		private Default317Buffer metadataBuffer;

		void Start()
		{
			//Load the index.dat once.
			archiveMedia = Client.requestArchive(4, "2d graphics", "media", 0, 40);
			metadataBuffer = new Default317Buffer(archiveMedia.decompressFile("index.dat"));
		}

		void Update()
		{
			//Returns true when it's finally done.
			if(Client.QueueUpAllMediaUnpacking(this, archiveMedia, metadataBuffer))
			{
				UnityEngine.GameObject.Destroy(this.gameObject);
			}
		}
	}
}
