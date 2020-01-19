using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class WebGLInterfaceUnpackLoaderHack : MonoBehaviour
	{
		private Archive archiveInterface;
		private Archive archiveMedia;
		private GameFont[] fonts;

		public RsUnityWebGLClient Client { get; set; }

		void Start()
		{
			archiveInterface = Client.requestArchive(3, "interface", "interface", 0, 35);
			archiveMedia = Client.requestArchive(4, "2d graphics", "media", 0, 40);

			GameFont fontFancy = new GameFont("q8_full", Client.archiveTitle, true);
			fonts = new GameFont[] { Client.fontSmall, Client.fontPlain, Client.fontBold, fontFancy };
		}

		void Update()
		{
			//Returns true when it's finally done.
			if (Client.HandlePendingInterfaceUnpackingAsync(archiveInterface, archiveMedia, fonts))
			{
				UnityEngine.GameObject.Destroy(this.gameObject);
			}
		}
	}
}
