
public sealed class OnDemandData extends Cacheable {

	int dataType;

	byte buffer[];
	int id;
	boolean incomplete;
	int loopCycle;

	public OnDemandData() {
		incomplete = true;
	}
}
