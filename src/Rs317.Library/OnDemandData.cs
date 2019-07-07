using System;

namespace Rs317.Sharp
{
	public sealed class OnDemandData : Cacheable
	{
		public int dataType { get; set; }

		public byte[] buffer { get; private set; }
		int id;
		public bool incomplete { get; set; }
		public int loopCycle { get; set; }

		public OnDemandData()
		{
			incomplete = true;
		}

		public void InitializeBuffer(byte[] bytes)
		{
			buffer = bytes ?? throw new ArgumentNullException(nameof(bytes));
		}

		public void ClearBuffer()
		{
			buffer = null;
		}
	}
}
