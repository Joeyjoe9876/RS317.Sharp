using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public sealed class GameClientPayloadAttribute : WireDataContractBaseTypeAttribute
	{
		public GameClientPayloadAttribute(RsClientNetworkOperationCode operationCode) 
			: base((int)operationCode, typeof(BaseGameClientPayload))
		{
		}
	}
}
