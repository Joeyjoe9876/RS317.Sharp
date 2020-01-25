using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using GladMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class SetClientAuthResultEventListener : BaseSingleEventListenerInitializable<IAuthenticationResultRecievedEventSubscribable, AuthenticationResultEventArgs>
	{
		private RsUnityClient Client { get; }

		public SetClientAuthResultEventListener(IAuthenticationResultRecievedEventSubscribable subscriptionService, RsUnityClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnEventFired(object source, AuthenticationResultEventArgs args)
		{
			//We should just complete the authentication process.
			Client.HandleLoginSuccessful(ClientPrivilegeType.Administrator, false);
		}
	}
}
