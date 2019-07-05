
sealed class RSFrame : Frame
{
	private sealed RSApplet applet;

	public RSFrame(RSApplet applet, int width, int height)
	{
		this.applet = applet;
		setTitle("Jagex");
		setResizable(false);
		setVisible(true);
		toFront();
		setSize(width + 8, height + 28);
	}

	@Override
	public Graphics getGraphics()
	{
		Graphics graphics = super.getGraphics();
		graphics.translate(4, 24);
		return graphics;
	}

	@Override
	public void paint(Graphics graphics)
	{
		applet.paint(graphics);
	}

	@Override
	public void update(Graphics graphics)
	{
		applet.update(graphics);
	}
}
