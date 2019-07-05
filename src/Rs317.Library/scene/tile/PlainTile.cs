
public sealed class PlainTile
{
	public sealed int colourA;

	public sealed int colourB;
	public sealed int colourD;
	public sealed int colourC;
	public sealed int texture;
	public boolean flat;
	public sealed int colourRGB;

	public PlainTile(int colourA, int colourB, int colourC, int colourD, int colourRGB, int texture, boolean flat)
	{
		this.flat = true;
		this.colourA = colourA;
		this.colourB = colourB;
		this.colourD = colourD;
		this.colourC = colourC;
		this.texture = texture;
		this.colourRGB = colourRGB;
		this.flat = flat;
	}
}
