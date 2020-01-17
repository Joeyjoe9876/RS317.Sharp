using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace Rs317.Sharp
{
	public static class ConstantData
	{
		public static readonly int[] BITFIELD_MAX_VALUE;

		//TODO: Don't expose mutable
		public static int[][] APPEARANCE_COLOURS = new int[][] {
			new int[] {
				6798, 107, 10283, 16, 4797, 7744, 5799, 4634, 33697, 22433,
				2983, 54193
			}, new int[] {
				8741, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094, 10153,
				56621, 4783, 1341, 16578, 35003, 25239
			}, new int[] {
				25238, 8742, 12, 64030, 43162, 7735, 8404, 1701, 38430, 24094,
				10153, 56621, 4783, 1341, 16578, 35003
			}, new int[] {
				4626, 11146, 6439, 12, 4758, 10270
			}, new int[] {
				4550, 4537, 5681, 5673, 5790, 6806, 8076, 4574
			}
		};

		public static int[] BEARD_COLOURS = { 9104, 10275, 7595, 3610, 7975, 8526, 918, 38802, 24466, 10145, 58654, 5027,
			1457, 16565, 34991, 25486 };

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetAppearanceColor(int x, int y)
		{
			return APPEARANCE_COLOURS[x][y];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetAppearanceColorRowLength(int x)
		{
			return APPEARANCE_COLOURS[x].Length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetBeardColor(int index)
		{
			return BEARD_COLOURS[index];
		}
	}
}
