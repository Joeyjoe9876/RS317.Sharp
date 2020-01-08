using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rs317.Sharp
{
	public sealed class UnityRsInputDispatcherComponent : MonoBehaviour
	{
		private bool isStarted = false;

		private IInputCallbackSubscriber _inputSubscribable;

		public IInputCallbackSubscriber InputSubscribable
		{
			get => _inputSubscribable;
			set
			{
				if (value != null)
				{
					isStarted = true;
					_inputSubscribable = value;
					InitializeCallbacks();
				}
			}
		}

		private void InitializeCallbacks()
		{
			/*this.MouseMove += new EventHandler<MouseMoveEventArgs>(mouseMoved);
			this.KeyDown += new EventHandler<KeyboardKeyEventArgs>(keyPressed);
			this.KeyUp += new EventHandler<KeyboardKeyEventArgs>(keyReleased);
			this.KeyPress += new EventHandler<KeyPressEventArgs>(keyTyped);*/
		}

		void Update()
		{
			if (!isStarted)
				return;

			//TODO: Enable mouse enter and leave event.
			//https://answers.unity.com/questions/973606/how-can-i-tell-if-the-mouse-is-over-the-game-windo.html
			//this.MouseEnter += new EventHandler<EventArgs>(mouseEntered);
			//this.MouseLeave += new EventHandler<EventArgs>(mouseExited);
			/*var view = camera.ScreenToViewportPoint(Input.mousePosition);
			var isOutside = view.x < 0 || view.x > 1 || view.y < 0 || view.y > 1;*/

			//It's not my fault Unity hasn't switched to an event-based input system
			//for 10 fucking years.

			//mousePressed
			if (Input.GetMouseButtonDown(0)) //left
				mousePressed(this, MouseButton.LeftMouse);

			if(Input.GetMouseButtonDown(1)) //right
				mousePressed(this, MouseButton.RightMouse);

			if(Input.GetMouseButtonDown(2)) //middle
				mousePressed(this, MouseButton.MiddleMouse);

			//mouseReleased
			if(Input.GetMouseButtonDown(0)) //left
				mouseReleased(this, MouseButton.LeftMouse);

			if(Input.GetMouseButtonDown(1)) //right
				mouseReleased(this, MouseButton.RightMouse);

			if(Input.GetMouseButtonDown(2)) //middle
				mouseReleased(this, MouseButton.MiddleMouse);
		}

		private void mouseExited(object sender, EventArgs e)
		{
			InputSubscribable?.mouseExited(sender, e);
		}

		private void mouseEntered(object sender, EventArgs e)
		{
			InputSubscribable?.mouseEntered(sender, e);
		}

		private void focusLost(object sender, EventArgs e)
		{
			InputSubscribable?.focusLost(sender, e);
		}

		private void focusGained(object sender, EventArgs e)
		{
			InputSubscribable?.focusGained(sender, e);
		}

		/*private void keyTyped(object sender, KeyPressEventArgs e)
		{
			InputSubscribable?.keyTyped(sender, new RsKeyEventArgs(e.KeyChar, e.KeyChar));
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
				i = (int)e.ScanCode;
			}

			InputSubscribable?.keyReleased(sender, new RsKeyEventArgs(i, (char)i));
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

			InputSubscribable?.keyPressed(sender, new RsKeyEventArgs(i, (char)i));
		}*/

		private void mouseMoved(object sender, object e)
		{
			//InputSubscribable?.mouseMoved(sender, TransformMouseEventCoordinates(e));
		}

		private void mouseReleased(object sender, MouseButton e)
		{
			InputSubscribable?.mouseReleased(sender, TransformMouseEventCoordinates(e));
		}

		//TODO: Reimplement the drag event.
		/*private void mouseDragged(object sender, DragEventArgs e)
		{
			InputSubscribable?.mouseDragged(sender, new RsMousePositionChangeEventArgs(e.X, e.Y));
		}*/

		private void mousePressed(object sender, MouseButton e)
		{
			InputSubscribable?.mousePressed(sender, TransformMouseEventCoordinates(e));
		}

		private RsMouseInputEventArgs TransformMouseEventCoordinates(MouseButton args)
		{
			//This line below about Unity input position is important. Runescape starts on the top left, Unity is bottom left.
			//The bottom-left of the screen or window is at (0, 0). The top-right of the screen or window is at (Screen.width, Screen.height).
			Vector2 position = Input.mousePosition;
			position = new Vector2(position.x, Screen.height - position.y); //similar to drawing textures correctly, we invert height.

			if(Screen.width == 765 && Screen.height == 503)
				return new RsMouseInputEventArgs((int)position.x, (int)position.y, args == MouseButton.RightMouse);
			else
			{
				//Get current size modifier
				float widthModifier = (float)Screen.width / 765.0f;
				float heightModifier = (float)Screen.height / 503.0f;

				return new RsMouseInputEventArgs((int)((float)position.x / widthModifier), (int)((float)position.y / heightModifier), args == MouseButton.RightMouse);
			}
		}
	}
}
