
using System;
using System.Threading;

public sealed class MouseDetection : IRunnable
{
	private Client clientInstance;

	private readonly object syncObj = new object();

	public int[] coordsY;
	public bool running;
	public int[] coordsX;
	public int coordsIndex;

	public MouseDetection(Client client1)
	{
		coordsY = new int[500];
		running = true;
		coordsX = new int[500];
		clientInstance = client1;
	}

	public void run()
	{
		while(running)
		{
			lock(syncObj)
			{
				if(coordsIndex < 500)
				{
					coordsX[coordsIndex] = clientInstance.mouseX;
					coordsY[coordsIndex] = clientInstance.mouseY;
					coordsIndex++;
				}
			}
			try
			{
				Thread.Sleep(50);
			}
			catch(Exception _ex)
			{
			}
		}
	}
}
