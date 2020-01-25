using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class PasswordFieldClientAdapter : IUIText
	{
		public string Text
		{
			get => Client.EnteredPassword;
			set => Client.EnteredPassword = value;
		}

		private RsUnityClient Client { get; }

		public PasswordFieldClientAdapter(RsUnityClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}
