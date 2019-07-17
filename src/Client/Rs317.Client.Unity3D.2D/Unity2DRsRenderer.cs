using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Rs317.Sharp
{
	/// <summary>
	/// Component that handles rendering the gamescene
	/// through <see cref="Graphics"/>.DrawTexture.
	/// See: https://docs.unity3d.com/ScriptReference/Graphics.DrawTexture.html
	/// </summary>
	public sealed class Unity2DRsRenderer : MonoBehaviour
	{
		private static readonly object SyncObj = new object();

		//TODO: Handle better communication for render data/requests.
		//TODO: Maybe conver this to a queue.
		private static List<RenderRequestData> RenderData = new List<RenderRequestData>();

		private Dictionary<string, Texture2D> TextureMap { get; } = new Dictionary<string, Texture2D>();

		[SerializeField]
		private UnityEngine.GameObject TexturePrefab;

		[SerializeField]
		private Canvas RootCanvas;

		private void Update()
		{
			lock (SyncObj)
			{
				foreach (var data in RenderData)
				{
					if (!TextureMap.ContainsKey(data.Name))
					{
						var gameObject = UnityEngine.GameObject.Instantiate(TexturePrefab, RootCanvas.transform);
						RawImage image = gameObject.GetComponent<UnityEngine.UI.RawImage>();

						Texture2D texture = new Texture2D((int)data.Location.width, (int)data.Location.height, TextureFormat.ARGB32, false);
						TextureMap[data.Name] = texture;
						image.texture = texture;

						data.OnTextureIntialCreation?.Invoke(texture.GetNativeTexturePtr());
						image.SetAllDirty();
					}
				}

				RenderData.Clear();
			}
		}

		public static void EnqueueRenderData(RenderRequestData data)
		{
			lock (SyncObj)
			{
				RenderData.Add(data);
			}
		}
	}
}
