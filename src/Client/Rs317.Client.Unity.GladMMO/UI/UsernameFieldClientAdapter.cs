using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class UsernameFieldClientAdapter : IUIText
	{
		public string Text
		{
			get => Client.EnteredUsername;
			set => Client.EnteredUsername = value;
		}

		private RsUnityClient Client { get; }

		public UsernameFieldClientAdapter(RsUnityClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}