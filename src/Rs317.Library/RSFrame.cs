
sealed class RSFrame : Frame
{
	private final RSApplet applet;

	public RSFrame(RSApplet applet, int width, int height)
	{
		this.applet = applet;
		setTitle("Jagex");
		setResizable(false);
		setVisible(true);
		toFront();
		setSize(width + 8, height + 28);
	}

	public override Graphics getGraphics()
	{
		Graphics graphics = super.getGraphics();
		graphics.translate(4, 24);
		return graphics;
	}

	public override void paint(Graphics graphics)
	{
		applet.paint(graphics);
	}

	public override void update(Graphics graphics)
	{
		applet.update(graphics);
	}
}
