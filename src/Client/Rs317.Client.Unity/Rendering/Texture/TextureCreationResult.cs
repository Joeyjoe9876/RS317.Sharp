using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class TextureCreationResult
	{
		public Texture2D Texture { get; }

		public NativeArray<int> NativePtr { get; set; }

		public TextureCreationResult(Texture2D texture)
		{
			Texture = texture;
			NativePtr = texture.GetRawTextureData<int>();
		}
	}
}
