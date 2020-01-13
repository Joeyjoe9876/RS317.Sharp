using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Rs317.GladMMO
{
	public sealed class UsernameFieldClientAdapter : IUIText
	{
		public string Text
		{
			get => Client.EnteredUsername;
			set => Client.EnteredUsername = value;
		}

		private GladMMOUnityClient Client { get; }

		public UsernameFieldClientAdapter(GladMMOUnityClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}