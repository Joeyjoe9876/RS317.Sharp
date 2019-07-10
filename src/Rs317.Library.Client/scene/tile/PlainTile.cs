namespace Rs317.Sharp
{
	public sealed class PlainTile
	{
		public int colourA;

		public int colourB;
		public int colourD;
		public int colourC;
		public int texture;
		public bool flat;
		public int colourRGB;

		public PlainTile(int colourA, int colourB, int colourC, int colourD, int colourRGB, int texture, bool flat)
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
}
