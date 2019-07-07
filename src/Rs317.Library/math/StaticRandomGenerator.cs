using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rs317.Sharp
{
	public static class StaticRandomGenerator
	{
		//TODO: If we do any async/await this will potentially fail? Maybe? TODO look into it.
		//Unique per thread.
		private static readonly System.Random internalRandomGenerator;

		static StaticRandomGenerator()
		{
			internalRandomGenerator = new System.Random();
		}

		//We must emulate this exactly.
		//https://docs.oracle.com/javase/7/docs/api/java/lang/Math.html#random()
		/*
		 *Returns a double value with a positive sign, greater than or equal to 0.0 and less than 1.0. Returned values are chosen pseudorandomly with (approximately) uniform distribution from that range.
			When this method is first called, it creates a single new pseudorandom-number generator, exactly as if by the expression
	
			new java.util.Random()
			This new pseudorandom-number generator is used thereafter for all calls to this method and is used nowhere else.
			This method is properly synchronized to allow correct use by more than one thread. However, if many threads need to generate pseudorandom numbers at a great rate, it may reduce contention for each thread to have its own pseudorandom-number generator.
		 */
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static double Next()
		{
			//Thankfully, C# NextDouble actually works like Java
			return internalRandomGenerator.NextDouble();
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static int Next(int max)
		{
			//.NET random doesn't support anything less than 0.
			if(max < 0) throw new ArgumentOutOfRangeException(nameof(max));

			return internalRandomGenerator.Next(max);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static double NextInt()
		{
			return internalRandomGenerator.NextDouble();
		}
	}
}
