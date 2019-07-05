
public sealed class Item : Animable
{
	public int itemId;

	public int x;

	public int y;
	public int itemCount;

	public Item()
	{
	}

	public sealed override Model getRotatedModel()
	{
		ItemDefinition itemDef = ItemDefinition.getDefinition(itemId);
		return itemDef.getAmountModel(itemCount);
	}
}
