using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rs317.Sharp
{
	/// <summary>
	/// The winform that the RS engine will use to render.
	/// </summary>
	public sealed class RsWinForm : Form
	{
		private IInputCallbackSubscriber InputSubscriber { get; set; }

		private KeysConverter KeyCodeConverter { get; }

		public RsWinForm(int width, int height)
		{
			KeyCodeConverter = new KeysConverter();
			this.DoubleBuffered = true;
			this.ClientSize = new System.Drawing.Size(width, height);
			SetupGameEventCallbacks();
		}

		public void RegisterInputSubscriber(IInputCallbackSubscriber subscriber)
		{
			InputSubscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
		}

		private void SetupGameEventCallbacks()
		{
			this.MouseDown += new MouseEventHandler(mousePressed);
			this.DragDrop += new DragEventHandler(mouseDragged);
			this.MouseUp += new MouseEventHandler(mouseReleased);
			this.MouseMove += new MouseEventHandler(mouseMoved);
			this.KeyDown += new KeyEventHandler(keyPressed);
			this.KeyUp += new KeyEventHandler(keyReleased);
			this.KeyPress += new KeyPressEventHandler(keyTyped);

			this.GotFocus += new EventHandler(focusGained);
			this.LostFocus += new EventHandler(focusLost);
			this.MouseEnter += new EventHandler(mouseEntered);
			this.MouseLeave += new EventHandler(mouseExited);
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

		private void keyReleased(object sender, KeyEventArgs e)
		{
			int i = (int)e.KeyValue;
			if(e.KeyCode == Keys.Enter)
				i = 10;
			else if(e.KeyCode == Keys.Left)
				i = 37;
			else if(e.KeyCode == Keys.Right)
				i = 39;
			else if(e.KeyCode == Keys.Up)
				i = 38;
			else if(e.KeyCode == Keys.Down)
				i = 40;
			else if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
				i = 8;
			else if(e.KeyCode == Keys.Control)
				i = 5;
			else if(e.KeyCode == Keys.Tab)
				i = 9;
			else if(e.KeyValue >= (int)Keys.F1 && e.KeyValue <= (int)Keys.F12)
				i = 1008 + e.KeyValue - (int)Keys.F1;
			else if(e.KeyCode == Keys.Home)
				i = 1000;
			else if(e.KeyCode == Keys.End)
				i = 1001;
			else if(e.KeyCode == Keys.PageUp)
				i = 1002;
			else if(e.KeyCode == Keys.PageDown)
				i = 1003;
			else
			{
				//TODO: This is bad for performance.
				string s = KeyCodeConverter.ConvertToString(e.KeyCode);
				if(s.Length > 0)
					i = s[0];
			}

			InputSubscriber?.keyReleased(sender, new RsKeyEventArgs(i, (char)i));
		}

		private void keyPressed(object sender, KeyEventArgs e)
		{
			int i = 0;
			if(e.KeyCode == Keys.Enter)
				i = 10;
			else if(e.KeyCode == Keys.Left)
				i = 1;
			else if(e.KeyCode == Keys.Right)
				i = 2;
			else if(e.KeyCode == Keys.Up)
				i = 3;
			else if(e.KeyCode == Keys.Down)
				i = 4;
			else if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
				i = 8;
			else if(e.KeyCode == Keys.Control)
				i = 5;
			else if(e.KeyCode == Keys.Tab)
				i = 9;
			else if(e.KeyValue >= (int)Keys.F1 && e.KeyValue <= (int)Keys.F12)
				i = 1008 + e.KeyValue - (int)Keys.F1;
			else if(e.KeyCode == Keys.Home)
				i = 1000;
			else if(e.KeyCode == Keys.End)
				i = 1001;
			else if(e.KeyCode == Keys.PageUp)
				i = 1002;
			else if(e.KeyCode == Keys.PageDown)
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
			InputSubscriber?.mouseReleased(sender, new RsMouseInputEventArgs(e.X, e.Y, e.Button == MouseButtons.Right));
		}

		private void mouseDragged(object sender, DragEventArgs e)
		{
			InputSubscriber?.mouseDragged(sender, new RsMousePositionChangeEventArgs(e.X, e.Y));
		}

		private void mousePressed(object sender, MouseEventArgs e)
		{
			InputSubscriber?.mousePressed(sender, new RsMouseInputEventArgs(e.X, e.Y, e.Button == MouseButtons.Right));
		}
	}
}
