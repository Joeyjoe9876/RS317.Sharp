
sealed class MouseDetection : Runnable
{
	private Client clientInstance;

	public Object syncObject;

	public int[] coordsY;
	public bool running;
	public int[] coordsX;
	public int coordsIndex;

	public MouseDetection(Client client1)
	{
		syncObject = new Object();
		coordsY = new int[500];
		running = true;
		coordsX = new int[500];
		clientInstance = client1;
	}

	public override void run()
	{
		while(running)
		{
			synchronized(syncObject) {
				if(coordsIndex < 500)
				{
					coordsX[coordsIndex] = clientInstance.mouseX;
					coordsY[coordsIndex] = clientInstance.mouseY;
					coordsIndex++;
				}
			}
			try
			{
				Thread.sleep(50L);
			}
			catch(Exception _ex)
			{
			}
		}
	}
}
