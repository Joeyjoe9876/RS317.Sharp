using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rs317.Sharp
{
	public sealed class UnityRsInputDispatcherComponent : MonoBehaviour
	{
		private bool isStarted = false;

		private IInputCallbackSubscriber _inputSubscribable;

		//don't include deletes, we handle those seperately.
		private static KeyCode[] KeyCodes { get; } = ((KeyCode[])System.Enum.GetValues(typeof(KeyCode)))
			.Where(k => k != KeyCode.Backspace && k != KeyCode.Delete)
			.ToArray();

		[SerializeField]
		private float BackSpaceRateSeconds = 0.1f;

		[SerializeField]
		private float WaitBeforeContinousBackspaceSeconds = 0.5f;

		/// <summary>
		/// Indicates the last absolute gametime the backspace was pressed.
		/// </summary>
		private float lastBackspacePress = 0.0f;

		private bool isHoldingBackspace = false;

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
			bool isLeftMouseDown = false;
			if (Input.GetMouseButtonDown(0)) //left
			{
				isLeftMouseDown = true;
				mousePressed(this, MouseButton.LeftMouse);
			}

			if(Input.GetMouseButtonDown(1)) //right
				mousePressed(this, MouseButton.RightMouse);

			//mouseReleased
			if(Input.GetMouseButtonUp(0)) //left
				mouseReleased(this, MouseButton.LeftMouse);

			if(Input.GetMouseButtonUp(1)) //right
				mouseReleased(this, MouseButton.RightMouse);

			if(Input.GetMouseButtonUp(2)) //middle
				mouseReleased(this, MouseButton.MiddleMouse);


			//this.MouseMove += new EventHandler<MouseMoveEventArgs>(mouseMoved);
			//If any axis has input it's moved.
			if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
			{
				//If the mouse has moved and the left button is being held down then we are dragging.
				if (isLeftMouseDown)
				{
					InputSubscribable?.mouseDragged(this, TransformMouseEventCoordinates(MouseButton.LeftMouse));
				}

				//It's important that this is called BEFORE mouseMoved otherwise the mouse position diff
				//calculated internally will be 0.
				//Is middle mouse being held down as the mouse moves
				if (Input.GetMouseButton(2))
				{
					InputSubscribable?.mouseWheelDragged(this, TransformMouseEventCoordinates(MouseButton.MiddleMouse));
				}

				mouseMoved();
			}

			//Invert specifically because the Unity3D implementation
			//gives inverted results as expected for scaling.
			if(Math.Abs(Input.mouseScrollDelta.y) > float.Epsilon)
				InputSubscribable?.mouseWheelScroll(this, -Input.mouseScrollDelta.y);
				

			//TODO: Make sperate input implementations
			//On mobile right now we handle keyboard input differently.
			if(!RsUnityPlatform.isAndroidMobileBuild)
				PollKeyboardInput();
		}

		private void PollKeyboardInput()
		{
			//This is ridiculous, but it's the only way to detect which keys are pressed.
			foreach (KeyCode keyCode in KeyCodes)
			{
				//All three events matter
				/*this.KeyDown += new EventHandler<KeyboardKeyEventArgs>(keyPressed);
				this.KeyUp += new EventHandler<KeyboardKeyEventArgs>(keyReleased);
				this.KeyPress += new EventHandler<KeyPressEventArgs>(keyTyped);*/

				//We can be smart-ish here
				//Keydown should be called for ALL keys that are down.
				if (Input.GetKeyDown(keyCode))
				{
					keyPressed(this, keyCode);
				}
				else if (Input.GetKeyUp(keyCode)) //can't be both
					keyReleased(this, keyCode);
			}

			//For typing we can iterate all typed keys since last frame.
			//https://docs.unity3d.com/ScriptReference/Input-inputString.html
			for (int i = 0; i < Input.inputString.Length; i++)
				keyTyped(this, Input.inputString[i]);

			//The inputstring will not have the backspace
			//if it's being held so we must do some special handling for
			//it so players in the rsclient can hold backspace.
			if (Input.GetKey(KeyCode.Backspace) || Input.GetKey(KeyCode.Delete))
			{
				if (isHoldingBackspace)
				{
					//It's being held down and therefore
					//lastBackspacePress actually is the last time it was pressed.
					//From there we can determine how many times we need to backspace
					int backspaceCount = (int) ((Time.time - lastBackspacePress) / BackSpaceRateSeconds);
					for (int i = 0; i < backspaceCount; i++)
						keyPressed(this, KeyCode.Backspace);

					if (backspaceCount > 0)
						lastBackspacePress = Time.time;
				}
				else
				{
					isHoldingBackspace = true;
					keyPressed(this, KeyCode.Backspace);
					lastBackspacePress = Time.time + WaitBeforeContinousBackspaceSeconds; //the added wait time will prevent us from pressing more than once until that cutoff.
				}
			}

			if (Input.GetKeyUp(KeyCode.Backspace) || Input.GetKeyUp(KeyCode.Delete))
			{
				isHoldingBackspace = false;
				lastBackspacePress = Time.time;
			}
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

		private void keyTyped(object sender, char typedChar)
		{
			InputSubscribable?.keyTyped(sender, new RsKeyEventArgs(typedChar, typedChar));
		}

		private void keyReleased(object sender, KeyCode e)
		{
			int i = (int)e;
			if(e == KeyCode.Return)
				i = 10;
			else if(e == KeyCode.LeftArrow)
				i = 37;
			else if(e == KeyCode.RightArrow)
				i = 39;
			else if(e == KeyCode.UpArrow)
				i = 38;
			else if(e == KeyCode.DownArrow)
				i = 40;
			else if(e == KeyCode.Backspace || e == KeyCode.Delete)
				i = 8;
			else if(e == KeyCode.LeftControl || e == KeyCode.RightControl)
				i = 5;
			else if(e == KeyCode.Tab)
				i = 9;
			else if((int)e >= (int)KeyCode.F1 && (int)e <= (int)KeyCode.F12)
				i = 1008 + (int)e - (int)KeyCode.F1;
			else if(e == KeyCode.Home)
				i = 1000;
			else if(e == KeyCode.End)
				i = 1001;
			else if(e == KeyCode.PageUp)
				i = 1002;
			else if(e == KeyCode.PageDown)
				i = 1003;
			else
			{
				//TODO: This is bad for performance.
				i = (int) e;
			}

			InputSubscribable?.keyReleased(sender, new RsKeyEventArgs(i, (char)i));
		}

		private void keyPressed(object sender, KeyCode e)
		{
			int i = 0;
			if(e == KeyCode.Return)
				i = 10;
			else if(e == KeyCode.LeftArrow)
				i = 1;
			else if(e == KeyCode.RightArrow)
				i = 2;
			else if(e == KeyCode.UpArrow)
				i = 3;
			else if(e == KeyCode.DownArrow)
				i = 4;
			else if(e == KeyCode.Backspace || e == KeyCode.Delete)
				i = 8;
			else if(e == KeyCode.LeftControl || e == KeyCode.RightControl)
				i = 5;
			else if(e == KeyCode.Tab)
				i = 9;
			else if((int)e >= (int)KeyCode.F1 && (int)e <= (int)KeyCode.F12)
				i = 1008 + (int)e - (int)KeyCode.F1;
			else if(e == KeyCode.Home)
				i = 1000;
			else if(e == KeyCode.End)
				i = 1001;
			else if(e == KeyCode.PageUp)
				i = 1002;
			else if(e == KeyCode.PageDown)
				i = 1003;
			else
				return;

			InputSubscribable?.keyPressed(sender, new RsKeyEventArgs(i, (char)i));
		}

		private void mouseMoved()
		{
			//TODO: Is this a hack to specify left-mouse on move?
			InputSubscribable?.mouseMoved(this, TransformMouseEventCoordinates(MouseButton.LeftMouse));
		}

		private void mouseReleased(object sender, MouseButton e)
		{
			InputSubscribable?.mouseReleased(sender, TransformMouseEventCoordinates(e));
		}

		//Unity calls this on window focus changes
		void OnApplicationFocus(bool hasFocus)
		{
			if(hasFocus)
				InputSubscribable?.focusGained(this, EventArgs.Empty);
			else
				InputSubscribable?.focusLost(this, EventArgs.Empty);
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
