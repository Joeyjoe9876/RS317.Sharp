using System;
using System.Collections.Generic;
using System.Text;
using GladNet;
using Rs317.Sharp;

namespace Rs317.Extended
{
	/// <summary>
	/// Simplified type interface for the <see cref="IFactoryCreateable{TCreateType,TContextType}"/>
	/// for managed client sessions.
	/// </summary>
	public interface IManagedClientSessionFactory : IFactoryCreateable<ManagedClientSession<BaseGameServerPayload, BaseGameClientPayload>, ManagedClientSessionCreationContext>
	{

	}
}