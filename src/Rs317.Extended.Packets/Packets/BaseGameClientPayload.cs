using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[WireDataContract(WireDataContractAttribute.KeyType.Byte, true)]
	public abstract class BaseGameClientPayload : IGamePacketPayload
	{
		
	}
}
