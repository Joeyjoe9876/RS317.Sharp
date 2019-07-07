using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rs317.Sharp
{
	public class RSApplet : Form, IRunnable, IRunnableStarter, IMouseInputQueryable //Instead of that Java stuff we just implement Windows form.
	{
		private int gameState;

		private int delayTime;

		int minDelay;

		private long[] otims;

		public int fps { get; private set; }

		public bool debugRequested { get; set; }

		protected int width { get; private set; }

		protected int height { get; private set; }

		protected Graphics gameGraphics { get; private set; }

		protected RSImageProducer fullGameScreen { get; set; }

		private bool clearScreen;

		protected bool awtFocus { get; set; }

		protected int idleTime { get; set; }

		protected int mouseButton { get; private set; }

		//IMouseInputQueryable
		public int mouseX { get; private set; }

		public int mouseY { get; private set; }

		private int eventMouseButton;

		private int eventClickX;

		private int eventClickY;

		private long eventClickTime;

		public int clickType { get; protected set; }

		protected int clickX { get; private set; }

		protected int clickY { get; private set; }

		protected long clickTime { get; private set; }

		protected int[] keyStatus { get; private set; }

		private int[] inputBuffer;

		private int readIndex;

		private int writeIndex;

		protected RSApplet()
		{
			delayTime = 20;
			minDelay = 1;
			otims = new long[10];
			debugRequested = false;
			clearScreen = true;
			awtFocus = true;
			keyStatus = new int[128];
			inputBuffer = new int[128];
		}

		protected virtual void cleanUpForQuit()
		{
		}

		public void createClientFrame(int width, int height)
		{
			this.width = width;
			this.height = height;

			signlink.applet = this;
			this.DoubleBuffered = true;
			this.ClientSize = new System.Drawing.Size(width, height);
			gameGraphics = CreateGraphics();
			fullGameScreen = new RSImageProducer(this.width, height, this);
			StartRunnable(this, 1);
			Application.Run(this);
		}

		protected virtual void drawLoadingText(int percentage, String s)
		{
			while (gameGraphics == null)
			{
				gameGraphics = this.CreateGraphics();
				try
				{
					Invalidate();
				}
				catch (Exception _ex)
				{
				}

				try
				{
					Thread.Sleep(1000);
				}
				catch (Exception _ex)
				{
				}
			}

			Font helveticaBold = new Font("Helvetica", 13, FontStyle.Bold);
			Font helvetica = new Font("Helvetica", 13, FontStyle.Regular);

			if (clearScreen)
			{
				gameGraphics.FillRectangle(Brushes.Black, 0, 0, width, height);
				clearScreen = false;
			}

			SolidBrush color = new SolidBrush(Color.FromArgb(140, 17, 17));
			Pen penColor = new Pen(color);

			int centerHeight = height / 2 - 18;

			gameGraphics.DrawRectangle(penColor, width / 2 - 152, centerHeight, 304, 34);
			gameGraphics.FillRectangle(color, width / 2 - 150, centerHeight + 2, percentage * 3, 30);
			gameGraphics.FillRectangle(Brushes.Black, (width / 2 - 150) + percentage * 3, centerHeight + 2, 300 - percentage * 3, 30);
			gameGraphics.DrawString(s, helveticaBold, Brushes.White, (width - gameGraphics.MeasureString(s, helveticaBold).Width) / 2, centerHeight + 22);
		}

		public void update(object sender, InvalidateEventArgs e)
		{
			//if (graphics == null)
			//	graphics = this.CreateGraphics();
			clearScreen = true;
			raiseWelcomeScreen();
		}

		public void paint(object sender, PaintEventArgs e)
		{
			if (gameGraphics == null)
				gameGraphics = e.Graphics;
			clearScreen = true;
			raiseWelcomeScreen();
		}

		public void mousePressed(object sender, MouseEventArgs e)
		{
			int i = e.X;
			int j = e.Y;

			idleTime = 0;
			eventClickX = i;
			eventClickY = j;
			eventClickTime = TimeService.CurrentTimeInMilliseconds();
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				eventMouseButton = 2;
				mouseButton = 2;
			}
			else
			{
				eventMouseButton = 1;
				mouseButton = 1;
			}

			Console.WriteLine($"Mouse Process");
		}

		public void mouseReleased(object sender, MouseEventArgs e)
		{
			idleTime = 0;
			mouseButton = 0;
		}

		public void mouseClicked(object sender, MouseEventArgs e)
		{
		}

		public void mouseEntered(object sender, EventArgs e)
		{
		}

		public void mouseExited(object sender, EventArgs e)
		{
			idleTime = 0;
			mouseX = -1;
			mouseY = -1;
		}

		public void mouseDragged(object sender, DragEventArgs e)
		{
			int i = e.X;
			int j = e.Y;

			idleTime = 0;
			mouseX = i;
			mouseY = j;
		}

		public void mouseMoved(object sender, MouseEventArgs e)
		{
			int i = e.X;
			int j = e.Y;

			idleTime = 0;
			mouseX = i;
			mouseY = j;
		}

		public void keyPressed(object sender, KeyEventArgs e)
		{
			idleTime = 0;
			int i = (int) e.KeyValue;
			if (e.KeyCode == Keys.Enter)
				i = 10;
			else if (e.KeyCode == Keys.Left)
				i = 1;
			else if (e.KeyCode == Keys.Right)
				i = 2;
			else if (e.KeyCode == Keys.Up)
				i = 3;
			else if (e.KeyCode == Keys.Down)
				i = 4;
			else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
				i = 8;
			else if (e.KeyCode == Keys.Control)
				i = 5;
			else if (e.KeyCode == Keys.Tab)
				i = 9;
			else if (e.KeyValue >= (int) Keys.F1 && e.KeyValue <= (int) Keys.F12)
				i = 1008 + e.KeyValue - (int) Keys.F1;
			else if (e.KeyCode == Keys.Home)
				i = 1000;
			else if (e.KeyCode == Keys.End)
				i = 1001;
			else if (e.KeyCode == Keys.PageUp)
				i = 1002;
			else if (e.KeyCode == Keys.PageDown)
				i = 1003;
			else
				return;

			if (i > 0 && i < 128)
				keyStatus[i] = 1;
			if (i > 4)
			{
				inputBuffer[writeIndex] = i;
				writeIndex = writeIndex + 1 & 0x7f;
			}
		}

		private KeysConverter kc = new KeysConverter();

		public void keyReleased(object sender, KeyEventArgs e)
		{
			idleTime = 0;
			int i = (int) e.KeyValue;
			if (e.KeyCode == Keys.Enter)
				i = 10;
			else if (e.KeyCode == Keys.Left)
				i = 37;
			else if (e.KeyCode == Keys.Right)
				i = 39;
			else if (e.KeyCode == Keys.Up)
				i = 38;
			else if (e.KeyCode == Keys.Down)
				i = 40;
			else if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
				i = 8;
			else if (e.KeyCode == Keys.Control)
				i = 5;
			else if (e.KeyCode == Keys.Tab)
				i = 9;
			else if (e.KeyValue >= (int) Keys.F1 && e.KeyValue <= (int) Keys.F12)
				i = 1008 + e.KeyValue - (int) Keys.F1;
			else if (e.KeyCode == Keys.Home)
				i = 1000;
			else if (e.KeyCode == Keys.End)
				i = 1001;
			else if (e.KeyCode == Keys.PageUp)
				i = 1002;
			else if (e.KeyCode == Keys.PageDown)
				i = 1003;
			else
			{
				string s = kc.ConvertToString(e.KeyCode);
				if (s.Length > 0)
					i = s[0];
			}

			char c = (char) i;
			if (c < (char) 36)
				c = (char) 0;
			if (i == 37)
				c = (char) 1;
			if (i == 39)
				c = (char) 2;
			if (i == 38)
				c = (char) 3;
			if (i == 40)
				c = (char) 4;
			if (i == 17)
				c = (char) 5;
			if (i == 8)
				c = '\b';
			if (i == 127)
				c = '\b';
			if (i == 9)
				c = '\t';
			if (i == 10)
				c = '\n';
			if (c > 0 && c < (char) 200)
				keyStatus[c] = 0;
		}

		public void keyTyped(object sender, KeyPressEventArgs e)
		{
			idleTime = 0;
			int i = (int) e.KeyChar;
			if (e.KeyChar >= 32 && e.KeyChar <= 126)
			{
				inputBuffer[writeIndex] = i;
				writeIndex = writeIndex + 1 & 0x7f;
			}
		}

		public int readChar(int dummy)
		{
			while (dummy >= 0)
			{
				for (int j = 1; j > 0; j++) ;
			}

			int k = -1;
			if (writeIndex != readIndex)
			{
				k = inputBuffer[readIndex];
				readIndex = readIndex + 1 & 0x7f;
			}

			return k;
		}

		public void focusGained(object sender, EventArgs e)
		{
			awtFocus = true;
			clearScreen = true;
			raiseWelcomeScreen();
		}

		public void focusLost(object sender, EventArgs e)
		{
			awtFocus = false;
			for (int i = 0; i < 128; i++)
				keyStatus[i] = 0;
		}

		public void windowActivated(object sender, EventArgs e)
		{
		}

		public void windowClosed(object sender, FormClosedEventArgs e)
		{
		}

		public void windowClosing(object sender, FormClosingEventArgs e)
		{
			Application.Exit();
			//destroy();
		}

		public void windowDeactivated(object sender, EventArgs e)
		{
		}

		public void windowDeiconified(object sender, EventArgs e)
		{
		}

		public void windowIconified(object sender, EventArgs e)
		{
		}

		public void windowOpened(object sender, EventArgs e)
		{
		}

		public virtual void processGameLoop()
		{
		}

		public virtual void processDrawing()
		{
		}

		public virtual void raiseWelcomeScreen()
		{
		}

		public virtual void redraw()
		{
		}

		private void exit()
		{
			gameState = -2;
			cleanUpForQuit();
			Application.Exit();
		}

		public virtual void run()
		{
			this.MouseDown += new MouseEventHandler(mousePressed);
			this.DragDrop += new DragEventHandler(mouseDragged);
			this.MouseUp += new MouseEventHandler(mouseReleased);
			this.MouseMove += new MouseEventHandler(mouseMoved);
			this.MouseEnter += new EventHandler(mouseEntered);
			this.MouseLeave += new EventHandler(mouseExited);
			this.KeyDown += new KeyEventHandler(keyPressed);
			this.KeyUp += new KeyEventHandler(keyReleased);
			this.KeyPress += new KeyPressEventHandler(keyTyped);
			this.GotFocus += new EventHandler(focusGained);
			this.LostFocus += new EventHandler(focusLost);
			this.FormClosing += new FormClosingEventHandler(windowClosing);
			this.Paint += new PaintEventHandler(paint);
			this.Invalidated += new InvalidateEventHandler(update);
			drawLoadingText(0, "Loading...");
			startUp();
			int opos = 0;
			int ratio = 256;
			int delay = 1;
			int count = 0;
			int intex = 0;
			for (int otim = 0; otim < 10; otim++)
				otims[otim] = TimeService.CurrentTimeInMilliseconds();

			while (gameState >= 0)
			{
				if (gameState > 0)
				{
					gameState--;
					if (gameState == 0)
					{
						exit();
						return;
					}
				}

				int i2 = ratio;
				int j2 = delay;
				ratio = 300;
				delay = 1;
				long currentTime = TimeService.CurrentTimeInMilliseconds();
				if (otims[opos] == 0L)
				{
					ratio = i2;
					delay = j2;
				}
				else if (currentTime > otims[opos])
					ratio = (int) (2560 * delayTime / (currentTime - otims[opos]));

				if (ratio < 25)
					ratio = 25;
				if (ratio > 256)
				{
					ratio = 256;
					delay = (int) (delayTime - (currentTime - otims[opos]) / 10L);
				}

				if (delay > delayTime)
					delay = delayTime;
				otims[opos] = currentTime;
				opos = (opos + 1) % 10;
				if (delay > 1)
				{
					for (int otim = 0; otim < 10; otim++)
						if (otims[otim] != 0L)
							otims[otim] += delay;

				}

				if (delay < minDelay)
					delay = minDelay;
				try
				{
					Thread.Sleep(delay);
				}
				catch (Exception _ex) //TODO: Log
				{
					intex++;
				}

				for (; count < 256; count += ratio)
				{
					clickType = eventMouseButton;
					clickX = eventClickX;
					clickY = eventClickY;
					clickTime = eventClickTime;
					eventMouseButton = 0;
					processGameLoop();
					readIndex = writeIndex;
				}

				count &= 0xff;
				if (delayTime > 0)
					fps = (1000 * ratio) / (delayTime * 256);
				processDrawing();
				if (debugRequested)
				{
					Console.WriteLine("ntime:" + currentTime);
					for (int i = 0; i < 10; i++)
					{
						int otim = ((opos - i - 1) + 20) % 10;
						Console.WriteLine("otim" + otim + ":" + otims[otim]);
					}

					Console.WriteLine("fps:" + fps + " ratio:" + ratio + " count:" + count);
					Console.WriteLine("del:" + delay + " deltime:" + delayTime + " mindel:" + minDelay);
					Console.WriteLine("intex:" + intex + " opos:" + opos);
					debugRequested = false;
					intex = 0;
				}
			}

			if (gameState == -1)
				exit();
		}

		void setFrameRate(int frameRate)
		{
			delayTime = 1000 / frameRate;
		}

		public void StartRunnable(IRunnable runnable, int priority)
		{
			//Run it on the threadpool instead.
			Task.Factory.StartNew(runnable.run, priority < 1 ? TaskCreationOptions.LongRunning : TaskCreationOptions.None);
		}

		public virtual void startUp()
		{

		}
	}
}
