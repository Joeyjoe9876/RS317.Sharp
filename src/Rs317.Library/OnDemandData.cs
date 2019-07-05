
public sealed class OnDemandData : Cacheable
{
	int dataType;

	byte buffer[];
	int id;
	bool incomplete;
	int loopCycle;

	public OnDemandData()
	{
		incomplete = true;
	}
}
