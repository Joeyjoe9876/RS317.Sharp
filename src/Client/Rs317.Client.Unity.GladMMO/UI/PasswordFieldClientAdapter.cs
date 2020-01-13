using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;

namespace Rs317.GladMMO
{
	public sealed class PasswordFieldClientAdapter : IUIText
	{
		public string Text
		{
			get => Client.EnteredPassword;
			set => Client.EnteredPassword = value;
		}

		private GladMMOUnityClient Client { get; }

		public PasswordFieldClientAdapter(GladMMOUnityClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}
