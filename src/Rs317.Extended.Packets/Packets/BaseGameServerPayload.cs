using System;
using System.Collections.Generic;
using System.Text;
using FreecraftCore.Serializer;

namespace Rs317.Extended
{
	[DefaultChild(typeof(UnknownServerGamePayload))]
	[WireDataContract(WireDataContractAttribute.KeyType.Byte, true)]
	public abstract class BaseGameServerPayload : IGamePacketPayload
	{
		/// <summary>
		/// Serializer ctor.
		/// </summary>
		protected BaseGameServerPayload()
		{
			
		}
	}
}
