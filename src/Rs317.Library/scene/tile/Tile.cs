
public sealed class Tile : Linkable
{
	public int z;

	public int x;
	public int y;
	public int anInt1310;
	public PlainTile plainTile;
	public ShapedTile shapedTile;
	public Wall wall;
	public WallDecoration wallDecoration;
	public GroundDecoration groundDecoration;
	public GroundItemTile groundItemTile;
	public int entityCount;
	public InteractiveObject[] interactiveObjects;
	public int[] interactiveObjectsSize;
	public int interactiveObjectsSizeOR;
	public int logicHeight;
	public bool abool1322;
	public bool abool1323;
	public bool abool1324;
	public int anInt1325;
	public int anInt1326;
	public int anInt1327;
	public int anInt1328;
	public Tile tileBelow;

	public Tile(int i, int j, int k)
	{
		interactiveObjects = new InteractiveObject[5];
		interactiveObjectsSize = new int[5];
		anInt1310 = z = i;
		x = j;
		y = k;
	}
}
