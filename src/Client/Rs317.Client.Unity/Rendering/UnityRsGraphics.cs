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

		[SerializeField]
		private bool ReorderDrawablesOnUpdate = true;

		[SerializeField]
		private Material OptionalRenderMaterial;

		[SerializeField]
		private int DrawableSize = 0;

		[SerializeField]
		private int InGameDrawableSize = 0;

		void Start()
		{
			if(OptionalRenderMaterial == null)
				Debug.LogWarning($"Warning. If {nameof(OptionalRenderMaterial)} on {nameof(UnityRsGraphics)} is null the WebGL no-alpha critical optimization may not work, screen may not render.");
		}

		public void DrawImageToScreen(string imageName, int x, int y)
		{
			//WebGL doesn't need to defer this to a different thread.
			if (RsUnityPlatform.isWebGLBuild)
			{
				UpdateDrawableTexture(imageName, x, y);
			}
			else
				DrawQueue.Enqueue(() => { UpdateDrawableTexture(imageName, x, y); });
		}

		private void UpdateDrawableTexture(string imageName, int x, int y)
		{
			TextureDrawable drawable = Drawables[imageName];
			drawable.X = x;
			drawable.Y = y;

			lock (SyncObj)
				TextureDictionary[imageName].Apply(false);

			if (ReorderDrawablesOnUpdate)
			{
				if(!IsIngameImage(imageName))
				{
					//TODO: Avoid shifting cost.
					//Remove from the list and then re-add
					//so that freshly updated textures are forced on top.
					DrawablesList.Remove(drawable);
					DrawablesList.Add(drawable);
				}
				else
				{
					//TODO: Avoid shifting cost.
					//Remove from the list and then re-add
					//so that freshly updated textures are forced on top.
					InGameDrawablesList.Remove(drawable);
					InGameDrawablesList.Add(drawable);
				}
			}
		}

		public Task<TextureCreationResult> CreateTexture(TextureCreationRequest request)
		{
			Debug.Log($"Created Texture: {request.Name}");

			TextureCreationRequestQueue.Enqueue(request);
			return request.GetCreationAwaitable();
		}

		private void Update()
		{
			//Client rendering can be turned off by things such as WebGL background throttling
			//so we should opt not to draw in those cases.
			if (!RsUnityClient.ShouldClientRender)
				return;

			DrawableSize = DrawablesList.Count;
			InGameDrawableSize = InGameDrawablesList.Count;

			//We need to create a texture if one is requested
			//has to be done on the main thread in Unity3D
			if (!TextureCreationRequestQueue.IsEmpty)
			{
				TextureCreationRequestQueue.TryDequeue(out var request);
				Texture2D texture = new Texture2D(request.Width, request.Height, TextureFormat.BGRA32, false, false);
				texture.wrapMode = TextureWrapMode.Clamp;

				//Point mode is important otherwise the filtering will cause bleeding by mips at the seams.
				//texture.filterMode = FilterMode.Point;

				TextureDictionary[request.Name] = texture;
				request.CompleteRequest(texture);

				//If we already have a drawable with this name
				//we need to remove it from the current draw list
				//This can happen going from the game back to the titlescreen
				//and etc.
				if (Drawables.ContainsKey(request.Name))
				{
					if(!IsIngameImage(request.Name))
					{
						DrawablesList.Remove(Drawables[request.Name]);
					}
					else
						InGameDrawablesList.Remove(Drawables[request.Name]);
				}

				Drawables[request.Name] = new TextureDrawable(texture, OptionalRenderMaterial);

				if(!IsIngameImage(request.Name))
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
			if (GameStateHookable == null)
				return;

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
			if(!GameStateHookable.LoggedIn)
				foreach(var drawable in DrawablesList)
					drawable.Draw();

			if(GameStateHookable.LoggedIn)
			{
				foreach(var drawable in InGameDrawablesList)
					drawable.Draw();
			}

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

			/*chatboxImageProducer = CreateNewImageProducer(479, 96, nameof(chatboxImageProducer));
			minimapImageProducer = CreateNewImageProducer(172, 156, nameof(minimapImageProducer));
			DrawingArea.clear();
			minimapBackgroundImage.draw(0, 0);
			tabImageProducer = CreateNewImageProducer(190, 261, nameof(tabImageProducer));
			gameScreenImageProducer = CreateNewImageProducer(512, 334, nameof(gameScreenImageProducer));
			DrawingArea.clear();
			chatSettingImageProducer = CreateNewImageProducer(496, 50, nameof(chatSettingImageProducer));
			bottomSideIconImageProducer = CreateNewImageProducer(269, 37, nameof(bottomSideIconImageProducer));
			topSideIconImageProducer = CreateNewImageProducer(249, 45, nameof(topSideIconImageProducer));*/

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

			switch (name)
			{
				case "chatboxImageProducer":
				case "minimapImageProducer":
				case "tabImageProducer":
				case "gameScreenImageProducer":
				case "chatSettingImageProducer":
				case "bottomSideIconImageProducer":
				case "topSideIconImageProducer":
					return true;
			}

			return false;
		}
	}
}
