using System;
using System.Collections.Generic;
using System.Text;
using GladNet;
using Rs317.Sharp;

namespace Rs317.Extended
{
	/// <summary>
	/// Simplified type interface for the <see cref="IFactoryCreateable{TCreateType,TContextType}"/>
	/// for managed sessions.
	/// </summary>
	public interface IManagedSessionFactory : IFactoryCreateable<IManagedNetworkServerClient<BaseGameServerPayload, BaseGameClientPayload>, ManagedSessionCreationContext>
	{

	}
}