using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	[RequireComponent(typeof(Camera))]
	public sealed class UnityRsGraphics : MonoBehaviour
	{
		private ConcurrentQueue<Action> DrawQueue { get; } = new ConcurrentQueue<Action>();

		private ConcurrentQueue<TextureCreationRequest> TextureCreationRequestQueue { get; } = new ConcurrentQueue<TextureCreationRequest>();

		private ConcurrentDictionary<string, Texture2D> TextureDictionary { get; } = new ConcurrentDictionary<string, Texture2D>();

		private ConcurrentDictionary<string, TextureDrawable> Drawables { get; } = new ConcurrentDictionary<string, TextureDrawable>();

		private List<TextureDrawable> DrawablesList { get; } = new List<TextureDrawable>();

		//The mutable sync object. Terrible decision, but I can too lazy to address the issues
		public object SyncObj { get; set; } = new object();

		public void DrawImageToScreen(string imageName, int x, int y)
		{
			Debug.Log($"DrawImageToScreen called");
			DrawQueue.Enqueue(() =>
			{
				Drawables[imageName].X = x;
				Drawables[imageName].Y = y;

				lock(SyncObj)
					TextureDictionary[imageName].Apply(false);

				//TODO: Avoid shifting cost.
				//Remove from the list and then re-add
				//so that freshly updated textures are forced on top.
				DrawablesList.Remove(Drawables[imageName]);
				DrawablesList.Add(Drawables[imageName]);
			});
		}

		public Task<TextureCreationResult> CreateTexture(TextureCreationRequest request)
		{
			TextureCreationRequestQueue.Enqueue(request);
			return request.GetCreationAwaitable();
		}

		private void Update()
		{
			//We need to create a texture if one is requested
			//has to be done on the main thread in Unity3D
			if (!TextureCreationRequestQueue.IsEmpty)
			{
				TextureCreationRequestQueue.TryDequeue(out var request);
				Texture2D texture = new Texture2D(request.Width, request.Height, TextureFormat.BGRA32, true, false);

				TextureDictionary[request.Name] = texture;
				request.CompleteRequest(texture);
				Drawables[request.Name] = new TextureDrawable(texture);
				DrawablesList.Add(Drawables[request.Name]);
			}
		}

		//Called after the scene has been rendered IF and ONLY IF this component
		//is attached to a camera.
		private void OnPostRender()
		{
			Debug.Log($"OnPostRender called");

			GL.PushMatrix();
			GL.LoadPixelMatrix();

			int count = DrawQueue.Count;

			for (int i = 0; i < count; i++)
			{
				Debug.Log($"Drawing OnPostRender");
				DrawQueue.TryDequeue(out Action drawAction);
				drawAction?.Invoke();
			}

			//Now it's time to actually draw all registered textures
			//use DrawablesList so we can semi-control order.
			foreach(var drawable in DrawablesList)
				drawable.Draw();

			GL.PopMatrix();
		}
	}
}
