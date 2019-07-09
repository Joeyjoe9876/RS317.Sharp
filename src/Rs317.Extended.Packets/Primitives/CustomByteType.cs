using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract]
	public abstract class CustomByteType
	{
		[WireMember(1)]
		protected byte ActualValue { get; private set; }

		public byte Value => ComputeCustomValue();

		protected CustomByteType(byte actualValue)
		{
			ActualValue = actualValue;
		}

		/// <summary>
		/// Serializer ctor.
		/// </summary>
		protected CustomByteType()
		{
			
		}

		protected abstract byte ComputeCustomValue();
	}
}
