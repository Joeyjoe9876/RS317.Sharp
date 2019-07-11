using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Rs317.Extended
{
	//TODO: Enable new ctor with an operation type enum.
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class ServerOperationTypeAttribute : SceneTypeCreateAttribute
	{
		public ServerOperationTypeAttribute()
			: base(1)
		{

		}
	}
}
