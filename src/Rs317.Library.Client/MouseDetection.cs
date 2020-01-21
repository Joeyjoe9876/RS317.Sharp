using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class MouseDetection : IRunnable
	{
		private IMouseInputQueryable MouseQueryable { get; }

		private readonly object syncObj = new object();

		public object SyncObj => syncObj;

		public int[] coordsY;
		public bool running;
		public int[] coordsX;
		public int coordsIndex;

		public MouseDetection(IMouseInputQueryable mouseQueryable)
		{
			if(mouseQueryable == null) throw new ArgumentNullException(nameof(mouseQueryable));
			MouseQueryable = mouseQueryable;
			coordsY = new int[500];
			running = true;
			coordsX = new int[500];
		}

		public async Task run()
		{
			while(running)
			{
				lock(syncObj)
				{
					if(coordsIndex < 500)
					{
						coordsX[coordsIndex] = MouseQueryable.mouseX;
						coordsY[coordsIndex] = MouseQueryable.mouseY;
						coordsIndex++;
					}
				}

				await Task.Delay(50);
			}
		}
	}
}
