using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	public class CustomByteA : CustomByteType
	{
		public CustomByteA(byte actualValue) 
			: base(actualValue)
		{

		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private CustomByteA()
		{
			
		}

		protected override byte ComputeCustomValue()
		{
			return (byte) (ActualValue - 128 & 0xff);
		}
	}
}
