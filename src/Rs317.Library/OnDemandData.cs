
public sealed class OnDemandData : Cacheable {

	int dataType;

	byte buffer[];
	int id;
	boolean incomplete;
	int loopCycle;

	public OnDemandData() {
		incomplete = true;
	}
}
