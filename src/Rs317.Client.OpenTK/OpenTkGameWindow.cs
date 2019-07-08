using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
	public sealed class OpenTKGameWindow : GameWindow
	{
		private bool ViewportSizeChanged = false;

		public static ConcurrentQueue<DrawImageQueueable> DrawImageQueue { get; } = new ConcurrentQueue<DrawImageQueueable>();

		private Dictionary<Bitmap, int> KnownBitmaps { get; }

		private Dictionary<int, DrawImageQueueable> ImageDrawCommands { get; }

		private IInputCallbackSubscriber InputSubscriber { get; set; }

		public OpenTKGameWindow(int width, int height)
			: base(width, height, GraphicsMode.Default, "Rs317.Sharp by Glader")
		{
			KnownBitmaps = new Dictionary<Bitmap, int>();
			ImageDrawCommands = new Dictionary<int, DrawImageQueueable>();
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
			GL.Enable(EnableCap.Blend);
			//GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcColor);

			GL.Viewport(0, 0, Width, Height);

			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref ortho_projection);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			if(ViewportSizeChanged)
			{
				ViewportSizeChanged = false;

				GL.Viewport(0, 0, Width, Height);

				Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, -1, 1);
				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadMatrix(ref ortho_projection);
			}

			while (DrawImageQueue.TryDequeue(out var drawRequest))
			{
				lock (drawRequest.Image)
				{
					if (!KnownBitmaps.ContainsKey(drawRequest.Image))
						CreateTexture(drawRequest);
					else
						UpdateTexture(drawRequest);
				}
			}

			foreach (KeyValuePair<int, DrawImageQueueable> imageRequest in ImageDrawCommands)
			{
				BindTexture(imageRequest.Key);
				DrawTexture(imageRequest.Value);
			}
				

			SwapBuffers();

			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
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

		private void DrawTexture(DrawImageQueueable drawRequest)
		{
			GL.Begin(PrimitiveType.Quads);

			float xOffset = (float) drawRequest.XDrawOffset;
			float yOffset = (float) drawRequest.XHeightOffset;

			//Scaling code for resizable
			//765, 503 default size.

			//Get current size modifier
			float widthModifier = (float)this.Width / 765.0f;
			float heightModifier = (float)this.Height / 503.0f;

			GL.TexCoord2(0, 0); GL.Vertex2(xOffset * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 0); GL.Vertex2((drawRequest.Width + xOffset) * widthModifier, yOffset * heightModifier);
			GL.TexCoord2(1, 1); GL.Vertex2((drawRequest.Width + xOffset) * widthModifier, (drawRequest.Height + yOffset) * heightModifier);
			GL.TexCoord2(0, 1); GL.Vertex2(xOffset * widthModifier, (drawRequest.Height + yOffset) * heightModifier);

			GL.End();
		}

		private void UpdateTexture(DrawImageQueueable drawRequest)
		{
			GL.BindTexture(TextureTarget.Texture2D, KnownBitmaps[drawRequest.Image]);
			BitmapData bmpData = drawRequest.Image.LockBits(new Rectangle(0, 0, drawRequest.Image.Width, drawRequest.Image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, bmpData.Width, bmpData.Height,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			drawRequest.Image.UnlockBits(bmpData); //Release the bitmap data from memory cause it is not needed anymore.
		}

		private void CreateTexture(DrawImageQueueable drawRequest)
		{
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			int texture;
			GL.GenTextures(1, out texture);
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
			KnownBitmaps.Add(drawRequest.Image, texture);
			ImageDrawCommands.Add(texture, drawRequest);

			BitmapData bmpData = drawRequest.Image.LockBits(new Rectangle(0, 0, drawRequest.Image.Width, drawRequest.Image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

			drawRequest.Image.UnlockBits(bmpData); //Release the bitmap data from memory cause it is not needed anymore.
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
			InputSubscriber?.mouseMoved(sender, new RsMousePositionChangeEventArgs(e.X, e.Y));
		}

		private void mouseReleased(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mouseReleased(sender, new RsMouseInputEventArgs(e.X, e.Y, e.Mouse.RightButton == ButtonState.Released));
		}

		//TODO: Reimplement the drag event.
		/*private void mouseDragged(object sender, DragEventArgs e)
		{
			InputSubscriber?.mouseDragged(sender, new RsMousePositionChangeEventArgs(e.X, e.Y));
		}*/

		private void mousePressed(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mousePressed(sender, new RsMouseInputEventArgs(e.X, e.Y, e.Mouse.RightButton == ButtonState.Pressed));
		}
	}
}
