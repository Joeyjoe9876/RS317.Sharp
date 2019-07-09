using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	public class CustomByteC : CustomByteType
	{
		public CustomByteC(byte actualValue) 
			: base(actualValue)
		{

		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		private CustomByteC()
		{
			
		}

		protected override byte ComputeCustomValue()
		{
			return (byte) (-ActualValue & 0xff);
		}
	}
}
