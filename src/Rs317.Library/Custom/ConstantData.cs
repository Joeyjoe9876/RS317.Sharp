using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace Rs317
{
	public static class ConstantData
	{
		public static readonly int[] BITFIELD_MAX_VALUE;

		static ConstantData()
		{
			BITFIELD_MAX_VALUE = new int[32];
			int totalExp = 2;
			for(int k = 0; k < 32; k++)
			{
				BITFIELD_MAX_VALUE[k] = totalExp - 1;
				totalExp += totalExp;
			}
		}

		//TODO: What even is this?
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetBitfieldMaxValue(int index)
		{
			return BITFIELD_MAX_VALUE[index];
		}
	}
}
