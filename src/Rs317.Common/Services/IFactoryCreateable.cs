using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IFactoryCreateable<out TCreateType, in TCreationContext>
	{
		TCreateType Create(TCreationContext context);
	}
}
