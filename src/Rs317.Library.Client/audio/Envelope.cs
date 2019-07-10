namespace Rs317.Sharp
{
	//Some of this file was refactored by 'veer' of http://www.moparscape.org.
	sealed class Envelope
	{
		private int phaseCount;

		private int[] phaseDuration;

		private int[] phasePeak;

		public int start { get; private set; }

		public int end { get; private set; }

		public int form { get; private set; }
		private int critical;
		private int phaseId;
		private int currentStep;
		private int amplitude;
		private int ticks;

		public void decode(Default317Buffer stream)
		{
			form = stream.getUnsignedByte();
			start = stream.getInt();
			end = stream.getInt();
			decodeShape(stream);
		}

		public void decodeShape(Default317Buffer stream)
		{
			phaseCount = stream.getUnsignedByte();
			phaseDuration = new int[phaseCount];
			phasePeak = new int[phaseCount];
			for(int p = 0; p < phaseCount; p++)
			{
				phaseDuration[p] = stream.getUnsignedLEShort();
				phasePeak[p] = stream.getUnsignedLEShort();
			}

		}

		public void resetValues()
		{
			critical = 0;
			phaseId = 0;
			currentStep = 0;
			amplitude = 0;
			ticks = 0;
		}

		//TODO: Rename this
		public int step(int period)
		{
			if(ticks >= critical)
			{
				amplitude = phasePeak[phaseId++] << 15;
				if(phaseId >= phaseCount)
					phaseId = phaseCount - 1;
				critical = (int)((phaseDuration[phaseId] / 65536D) * period);
				if(critical > ticks)
					currentStep = ((phasePeak[phaseId] << 15) - amplitude) / (critical - ticks);
			}

			amplitude += currentStep;
			ticks++;
			return amplitude - currentStep >> 15;
		}
	}
}
