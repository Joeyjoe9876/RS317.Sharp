using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public enum ClientLoginType : byte
	{
		CreateSession = 16,

		ClaimExistingSession = 18,
	}
}