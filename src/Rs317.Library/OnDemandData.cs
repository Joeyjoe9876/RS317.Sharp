
using System;

public sealed class OnDemandData : Cacheable
{
	public int dataType { get; private set; }

	byte[] buffer;
	int id;
	bool incomplete;
	int loopCycle;

	public OnDemandData()
	{
		incomplete = true;
	}

	public void InitializeBuffer(byte[] bytes)
	{
		buffer = bytes ?? throw new ArgumentNullException(nameof(bytes));
	}
}
