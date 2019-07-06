using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rs317
{
	public static class CollectionUtilities
	{
		/// <summary>
		/// Creates a new 3D jagged array with fully initialized arrays
		/// with dimensions <see cref="x"/> <see cref="y"/> <see cref="z"/>.
		/// x being first (not like RS).
		/// </summary>
		/// <typeparam name="TElementType"></typeparam>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TElementType[][][] Create3DJaggedArray<TElementType>(int x, int y, int z)
		{
			TElementType[][][] collection = new TElementType[x][][];

			for (int i = 0; i < collection.Length; i++)
			{
				collection[i] = Create2DJaggedArray<TElementType>(y, z);
			}

			return collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TElementType[][][][] Create4DJaggedArray<TElementType>(int x, int y, int z, int w)
		{
			TElementType[][][][] collection = new TElementType[x][][][];

			//Clever!
			for(int i = 0; i < collection.Length; i++)
				collection[i] = Create3DJaggedArray<TElementType>(y, z, w);

			return collection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TElementType[][] Create2DJaggedArray<TElementType>(int x, int y)
		{
			TElementType[][] collection = new TElementType[x][];

			for(int i = 0; i < collection.Length; i++)
				collection[i] = new TElementType[y];

			return collection;
		}
	}
}
