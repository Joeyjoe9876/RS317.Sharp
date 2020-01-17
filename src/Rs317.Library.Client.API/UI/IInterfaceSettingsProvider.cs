using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	//Honestly not sure what this is yet
	//But it basically allows for GameObjectDefinition to not hold a static ref to the client.
	public interface IInterfaceSettingsProvider
	{
		int GetInterfaceSettings(int index);
	}
}
