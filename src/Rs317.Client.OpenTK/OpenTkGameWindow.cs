using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Rs317.Sharp
{
	/// <summary>
	/// The Open GL through OpenTK game window for the game client.
	/// </summary>
	public sealed class OpenTKGameWindow : GameWindow, IImagePaintEventListener
	{
		private bool ViewportSizeChanged = false;

		private ConcurrentQueue<IOpenTKImageRenderable> ImageProducerCreationQueue { get; }

		private IInputCallbackSubscriber InputSubscriber { get; set; }

		private List<OpenGlRegisteredOpenTKImageRenderable> Renderables { get; }

		public OpenTKGameWindow(int width, int height)
			: base(width, height, new RsOpenTkGraphicsMocde(), "Rs317.Sharp by Glader")
		{
			ImageProducerCreationQueue = new ConcurrentQueue<IOpenTKImageRenderable>();
			Renderables = new List<OpenGlRegisteredOpenTKImageRenderable>(25);
			SetupGameEventCallbacks();
		}

		public void RegisterInputSubscriber(IInputCallbackSubscriber subscriber)
		{
			InputSubscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
		}

		private void SetupGameEventCallbacks()
		{
			//TODO: Reimplement mousedrag somehow.
			// this.DragDrop += new DragEventHandler(mouseDragged);

			this.MouseDown += new EventHandler<MouseButtonEventArgs>(mousePressed);
			this.MouseUp += new EventHandler<MouseButtonEventArgs>(mouseReleased);
			this.MouseMove += new EventHandler<MouseMoveEventArgs>(mouseMoved);
			this.KeyDown += new EventHandler<KeyboardKeyEventArgs>(keyPressed);
			this.KeyUp += new EventHandler<KeyboardKeyEventArgs>(keyReleased);
			this.KeyPress += new EventHandler<KeyPressEventArgs>(keyTyped);

			this.MouseEnter += new EventHandler<EventArgs>(mouseEntered);
			this.MouseLeave += new EventHandler<EventArgs>(mouseExited);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			GL.ClearColor(Color.MidnightBlue);
			GL.Enable(EnableCap.Texture2D);

			RecalculateViewPort();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);

			if(ViewportSizeChanged)
			{
				ViewportSizeChanged = false;

				RecalculateViewPort();
			}

			//All new image producers should have their OpenGL
			//data initialized
			//This should only be done ONCE per image producer, on creation.
			if (ImageProducerCreationQueue.TryDequeue(out var imageProducer))
			{
				//So, some producers may be created but not actually ready
				//for rendering purposes.
				//To avoid this issue we check if they are dirty.
				//If the queued image isn't dirty yet, then it's not ready to even be renderer.
				//We can requeue it and maybe handle it later.
				if (imageProducer.isDirty)
				{
					int textureId = CreateTexture(imageProducer);
					Renderables.Add(new OpenGlRegisteredOpenTKImageRenderable(imageProducer, textureId));
				}
				else
					ImageProducerCreationQueue.Enqueue(imageProducer);
			}

			int textureUploadCount = 0;
			//Now, for every renderable that we have registered
			//we just need to actually render it.
			foreach (var renderable in Renderables)
			{
				//TODO: Double check locking on isDirty
				//When the renderable is dirty, we must update
				//its representation in OpenGL
				if (renderable.Renderable.isDirty)
				{
					//For detecting which texture uploads are occuring.
					//Console.WriteLine($"Producer: {renderable.Renderable.Name} is dirty.");
					lock (renderable.Renderable.SyncObject)
					{
						//Renderable WILL still be dirty, definitely.
						//But now anything that attempts to set the dirty bit will have to wait.
						UpdateTexture(renderable);
						renderable.Renderable.ConsumeDirty();
						textureUploadCount++;
					}
				}

				//TODO: Avoid calling beind again if we did update the texture. (it's already bound).
				//Regardless of the result we need to now render the image
				BindTexture(renderable.TextureId);
				DrawTexture(renderable.Renderable);
			}

			SwapBuffers();

			//For debugging texture uploads.
			/*if(textureUploadCount != 0)
				Console.WriteLine($"Texture Uploads this Frame: {textureUploadCount}");*/
		}

		private void RecalculateViewPort()
		{
			GL.Viewport(0, 0, Width, Height);

			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref ortho_projection);
		}

		private void BindTexture(int textureId)
		{
			GL.BindTexture(TextureTarget.Texture2D, textureId);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			ViewportSizeChanged = true;
		}

		private void DrawTexture(IOpenTKImageRenderable renderable)
		{
			float xOffset = (float) renderable.ImageLocation.X;
			float yOffset = (float) renderable.ImageLocation.Y;

			//Scaling code for resizable
			//765, 503 default size.

			//Get current size modifier
			float widthModifier = (float)this.Width / 765.0f;
			float heightModifier = (float)this.Height / 503.0f;

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex2(xOffset * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 0); GL.Vertex2((renderable.ImageLocation.Width + xOffset) * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 1); GL.Vertex2((renderable.ImageLocation.Width + xOffset) * widthModifier, (renderable.ImageLocation.Height + yOffset) * heightModifier);
			GL.TexCoord2(0, 1); GL.Vertex2(xOffset * widthModifier, (renderable.ImageLocation.Height + yOffset) * heightModifier);
			GL.End();
		}

		private void UpdateTexture(OpenGlRegisteredOpenTKImageRenderable renderable)
		{
			GL.BindTexture(TextureTarget.Texture2D, renderable.TextureId);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, renderable.Renderable.ImageLocation.Width, renderable.Renderable.ImageLocation.Height,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, renderable.Renderable.ImageDataPointer);
		}

		/// <summary>
		/// Creates the texture in OpenGL.
		/// Returns the associated texture id.
		/// </summary>
		/// <param name="drawRequest"></param>
		/// <returns></returns>
		private int CreateTexture(IOpenTKImageRenderable drawRequest)
		{
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			int texture;
			GL.GenTextures(1, out texture);
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, drawRequest.ImageLocation.Width, drawRequest.ImageLocation.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, drawRequest.ImageDataPointer);

			return texture;
		}

		private void mouseExited(object sender, EventArgs e)
		{
			InputSubscriber?.mouseExited(sender, e);
		}

		private void mouseEntered(object sender, EventArgs e)
		{
			InputSubscriber?.mouseEntered(sender, e);
		}

		private void focusLost(object sender, EventArgs e)
		{
			InputSubscriber?.focusLost(sender, e);
		}

		private void focusGained(object sender, EventArgs e)
		{
			InputSubscriber?.focusGained(sender, e);
		}

		private void keyTyped(object sender, KeyPressEventArgs e)
		{
			InputSubscriber?.keyTyped(sender, new RsKeyEventArgs(e.KeyChar, e.KeyChar));
		}

		private void keyReleased(object sender, KeyboardKeyEventArgs e)
		{
			int i = (int)e.Key;
			if(e.Key == Key.Enter)
				i = 10;
			else if(e.Key == Key.Left)
				i = 37;
			else if(e.Key == Key.Right)
				i = 39;
			else if(e.Key == Key.Up)
				i = 38;
			else if(e.Key == Key.Down)
				i = 40;
			else if(e.Key == Key.Back || e.Key == Key.Delete)
				i = 8;
			else if(e.Key == Key.ControlLeft || e.Key == Key.ControlRight)
				i = 5;
			else if(e.Key == Key.Tab)
				i = 9;
			else if((int)e.Key >= (int)Key.F1 && (int)e.Key <= (int)Key.F12)
				i = 1008 + (int)e.Key - (int)Key.F1;
			else if(e.Key == Key.Home)
				i = 1000;
			else if(e.Key == Key.End)
				i = 1001;
			else if(e.Key == Key.PageUp)
				i = 1002;
			else if(e.Key == Key.PageDown)
				i = 1003;
			else
			{
				//TODO: This is bad for performance.
				i = (int) e.ScanCode;
			}

			InputSubscriber?.keyReleased(sender, new RsKeyEventArgs(i, (char)i));
		}

		private void keyPressed(object sender, KeyboardKeyEventArgs e)
		{
			int i = 0;
			if(e.Key == Key.Enter)
				i = 10;
			else if(e.Key == Key.Left)
				i = 1;
			else if(e.Key == Key.Right)
				i = 2;
			else if(e.Key == Key.Up)
				i = 3;
			else if(e.Key == Key.Down)
				i = 4;
			else if(e.Key == Key.Back || e.Key == Key.Delete)
				i = 8;
			else if(e.Key == Key.ControlLeft || e.Key == Key.ControlRight)
				i = 5;
			else if(e.Key == Key.Tab)
				i = 9;
			else if((int)e.Key >= (int)Key.F1 && (int)e.Key <= (int)Key.F12)
				i = 1008 + (int)e.Key - (int)Key.F1;
			else if(e.Key == Key.Home)
				i = 1000;
			else if(e.Key == Key.End)
				i = 1001;
			else if(e.Key == Key.PageUp)
				i = 1002;
			else if(e.Key == Key.PageDown)
				i = 1003;
			else
				return;

			InputSubscriber?.keyPressed(sender, new RsKeyEventArgs(i, (char)i));
		}

		private void mouseMoved(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mouseMoved(sender, TransformMouseEventCoordinates(e));
		}

		private void mouseReleased(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mouseReleased(sender, TransformMouseEventCoordinates(e));
		}

		//TODO: Reimplement the drag event.
		/*private void mouseDragged(object sender, DragEventArgs e)
		{
			InputSubscriber?.mouseDragged(sender, new RsMousePositionChangeEventArgs(e.X, e.Y));
		}*/

		private void mousePressed(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mousePressed(sender, TransformMouseEventCoordinates(e));
		}

		private RsMouseInputEventArgs TransformMouseEventCoordinates(MouseEventArgs args)
		{
			if(this.Width == 765 && Height == 503)
				return new RsMouseInputEventArgs(args.X, args.Y, args.Mouse.RightButton == ButtonState.Pressed);
			else
			{
				//Get current size modifier
				float widthModifier = (float)this.Width / 765.0f;
				float heightModifier = (float)this.Height / 503.0f;

				return new RsMouseInputEventArgs((int) ((float) args.X / widthModifier), (int) ((float) args.Y / heightModifier), args.Mouse.RightButton == ButtonState.Pressed);
			}
		}

		public void OnImageProducerCreated(IOpenTKImageRenderable imageProducer)
		{
			ImageProducerCreationQueue.Enqueue(imageProducer);
		}
	}
}
