using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IFactoryCreateable<in TCreationContext, out TCreateType>
	{
		TCreateType Create(TCreationContext context);
	}
}
