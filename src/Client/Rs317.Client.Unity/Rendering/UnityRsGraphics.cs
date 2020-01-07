using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

		private List<TextureDrawable> InGameDrawablesList { get; } = new List<TextureDrawable>();

		//The mutable sync object. Terrible decision, but I can too lazy to address the issues
		public object SyncObj { get; set; } = new object();

		//Must be set because it exists in the scene. Bad design choice by me, sorry.
		public IGameStateHookable GameStateHookable { get; set; }

		public void DrawImageToScreen(string imageName, int x, int y)
		{
			DrawQueue.Enqueue(() =>
			{
				Drawables[imageName].X = x;
				Drawables[imageName].Y = y;

				lock(SyncObj)
					TextureDictionary[imageName].Apply(false);

				if (!IsIngameImage(imageName))
				{
					//TODO: Avoid shifting cost.
					//Remove from the list and then re-add
					//so that freshly updated textures are forced on top.
					DrawablesList.Remove(Drawables[imageName]);
					DrawablesList.Add(Drawables[imageName]);
				}
				else
				{
					//TODO: Avoid shifting cost.
					//Remove from the list and then re-add
					//so that freshly updated textures are forced on top.
					InGameDrawablesList.Remove(Drawables[imageName]);
					InGameDrawablesList.Add(Drawables[imageName]);
				}
			});
		}

		public Task<TextureCreationResult> CreateTexture(TextureCreationRequest request)
		{
			Debug.Log($"Created Texture: {request.Name}");

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
				Texture2D texture = new Texture2D(request.Width, request.Height, TextureFormat.BGRA32, false, false);

				TextureDictionary[request.Name] = texture;
				request.CompleteRequest(texture);
				Drawables[request.Name] = new TextureDrawable(texture);

				if (!IsIngameImage(request.Name))
				{
					DrawablesList.Add(Drawables[request.Name]);
				}
				else
					InGameDrawablesList.Add(Drawables[request.Name]);
			}
		}

		//Called after the scene has been rendered IF and ONLY IF this component
		//is attached to a camera.
		private void OnPostRender()
		{
			GL.PushMatrix();
			GL.LoadPixelMatrix();

			int count = DrawQueue.Count;

			for (int i = 0; i < count; i++)
			{
				DrawQueue.TryDequeue(out Action drawAction);
				drawAction?.Invoke();
			}

			//Now it's time to actually draw all registered textures
			//use DrawablesList so we can semi-control order.
			foreach(var drawable in GameStateHookable.LoggedIn ? InGameDrawablesList : DrawablesList)
				drawable.Draw();

			GL.PopMatrix();
		}

		private bool IsIngameImage(string name)
		{
			//HelloKitty: This is kind of hacky, I know. BUT because of the way that the static frames
			//around the in-game UI work they don't recreate after login and logout. Unlike almost all other frames.
			//So we must handle them specially.

			/*Created ImageProducer: backLeftIP1
			Created ImageProducer: backLeftIP2
			Created ImageProducer: backRightIP1
			Created ImageProducer: backRightIP2
			Created ImageProducer: backTopIP1
			Created ImageProducer: backVmidIP1
			Created ImageProducer: backVmidIP2
			Created ImageProducer: backVmidIP3
			Created ImageProducer: backVmidIP2_2*/

			switch(name)
			{
				case "backLeftIP1":
				case "backLeftIP2":
				case "backRightIP1":
				case "backRightIP2":
				case "backTopIP1":
				case "backVmidIP1":
				case "backVmidIP2":
				case "backVmidIP3":
				case "backVmidIP2_2":
					return true;
			}

			return false;
		}
	}
}
