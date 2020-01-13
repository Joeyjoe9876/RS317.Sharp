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

		private GladMMOOpenTkClient Client { get; }

		public PasswordFieldClientAdapter(GladMMOOpenTkClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}
	}
}
