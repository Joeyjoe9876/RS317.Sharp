using System;
using System.Collections.Generic;
using System.Text;
using Glader.Essentials;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class SetClientAuthResultEventListener : BaseSingleEventListenerInitializable<IAuthenticationResultRecievedEventSubscribable, AuthenticationResultEventArgs>
	{
		private GladMMOOpenTkClient Client { get; }

		public SetClientAuthResultEventListener(IAuthenticationResultRecievedEventSubscribable subscriptionService, GladMMOOpenTkClient client) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		protected override void OnEventFired(object source, AuthenticationResultEventArgs args)
		{
			//We should just complete the authentication process.
			Client.HandleLoginSuccessful(3, false);
		}
	}
}
