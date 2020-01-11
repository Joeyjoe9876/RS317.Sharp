using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Rs317.Sharp
{
	public sealed class RsUnityClient : Client<UnityRsGraphics>
	{
		public UnityRsGraphics GraphicsObject { get; }

		private MonoBehaviour ClientMonoBehaviour { get; }

		public RsUnityClient(ClientConfiguration config, UnityRsGraphics graphicsObject, [NotNull] MonoBehaviour clientMonoBehaviour) 
			: base(config, new DefaultBufferFactory())
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
			ClientMonoBehaviour = clientMonoBehaviour ?? throw new ArgumentNullException(nameof(clientMonoBehaviour));

			//Only need to override this for WebGL.
			if(Application.platform == RuntimePlatform.WebGLPlayer)
				Sprite.ExternalLoadImageHook += ExternalLoadImageHook;
		}

		private int[] ExternalLoadImageHook(byte[] arg)
		{
			//LoadImage will replace with with incoming image size.
			Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			ImageConversion.LoadImage(tex, arg);
			return tex.GetPixels32().Select(ColorUtils.ColorToRGB).ToArray();
		}

		protected override IRSGraphicsProvider<UnityRsGraphics> CreateGraphicsProvider()
		{
			return new UnityRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<UnityRsGraphics> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			return new UnityRsImageProducer(xSize, ySize, producerName, gameGraphics);
		}

		public override void StartRunnable(IRunnable runnable, int priority)
		{
			if(Application.platform != RuntimePlatform.WebGLPlayer)
				base.StartRunnable(runnable, priority);
			else
			{
				//Webgl doesn't support threading very well, if at all
				//so we need to handle runnables differently
				ClientMonoBehaviour.StartCoroutine(RunnableRunCoroutine(runnable));
			}
		}

		private IEnumerator RunnableRunCoroutine(IRunnable runnable)
		{
			yield return new WaitForEndOfFrame();

			//Hope for the best, how it doesn't run infinitely or Unity will break.
			runnable.run();
		}

		public override void run()
		{
			//Non-webgl builds can run normally.
			if(Application.platform != RuntimePlatform.WebGLPlayer)
				base.run();
			else
			{
				//If it's webgl, we need to implement it on the main thread
				//as a coroutine. Otherwise WebGL will just not work due to
				//threading limitations.
				ClientMonoBehaviour.StartCoroutine(AppletRunCoroutine());
			}
		}

		public override void startUp()
		{
			base.startUp();
		}

		private IEnumerator AppletRunCoroutine()
		{
			Console.WriteLine($"Loading.");
			drawLoadingText(0, "Loading...");
			startUp();
			int opos = 0;
			int ratio = 256;
			int delay = 1;
			int count = 0;
			int intex = 0;
			for(int otim = 0; otim < 10; otim++)
				otims[otim] = TimeService.CurrentTimeInMilliseconds();

			while(gameState >= 0)
			{
				if(gameState > 0)
				{
					gameState--;
					if(gameState == 0)
					{
						exit();
						yield break;
					}
				}

				int i2 = ratio;
				int j2 = delay;
				ratio = 300;
				delay = 1;
				long currentTime = TimeService.CurrentTimeInMilliseconds();
				if(otims[opos] == 0L)
				{
					ratio = i2;
					delay = j2;
				}
				else if(currentTime > otims[opos])
					ratio = (int)(2560 * delayTime / (currentTime - otims[opos]));

				if(ratio < 25)
					ratio = 25;
				if(ratio > 256)
				{
					ratio = 256;
					delay = (int)(delayTime - (currentTime - otims[opos]) / 10L);
				}

				if(delay > delayTime)
					delay = delayTime;
				otims[opos] = currentTime;
				opos = (opos + 1) % 10;
				if(delay > 1)
				{
					for(int otim = 0; otim < 10; otim++)
						if(otims[otim] != 0L)
							otims[otim] += delay;

				}

				if(delay < minDelay)
					delay = minDelay;

				if(delay > 1)
					//Delay is in milliseconds, so we need to convert to seconds.
					yield return new WaitForSeconds((float)delay / 1000.0f);
				else if(delay == 1)
					yield return new WaitForEndOfFrame();

				for(; count < 256; count += ratio)
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
				if(delayTime > 0)
					fps = (1000 * ratio) / (delayTime * 256);
				processDrawing();
				if(debugRequested)
				{
					Console.WriteLine("ntime:" + currentTime);
					for(int i = 0; i < 10; i++)
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

			if(gameState == -1)
				exit();
		}
	}
}
