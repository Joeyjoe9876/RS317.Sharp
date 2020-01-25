using System;
using System.Collections.Generic;
using System.Text;
using GladMMO;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class InitializeDefaultLocalSpawnRequirementsEventListener : OnLocalPlayerSpawnedEventListener
	{
		private RsUnityClient Client { get; }

		private ILocalCharacterDataRepository CharacterDataRepository { get; }

		public InitializeDefaultLocalSpawnRequirementsEventListener(ILocalPlayerSpawnedEventSubscribable subscriptionService,
			[NotNull] RsUnityClient client,
			[NotNull] ILocalCharacterDataRepository characterDataRepository) 
			: base(subscriptionService)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			CharacterDataRepository = characterDataRepository ?? throw new ArgumentNullException(nameof(characterDataRepository));
		}

		protected override void OnLocalPlayerSpawned(LocalPlayerSpawnedEventArgs args)
		{
			Client.SetWalkableInterfaceId(-1);
			Client.SetNetworkPlayerStatus(1, CharacterDataRepository.CharacterId); //TODO: Don't just hackily add default 4.
			Client.ResetCutsceneCamera();
			Client.ResetInterfaceSettings();

			Client.LinkSideBarToInterface(3971, 1);
			Client.LinkSideBarToInterface(638, 2);
			Client.LinkSideBarToInterface(3213, 3);
			Client.LinkSideBarToInterface(1644, 4);
			Client.LinkSideBarToInterface(5608, 5);
			Client.LinkSideBarToInterface(1151, 6);
			Client.LinkSideBarToInterface(short.MaxValue, 7); //TODO: This one is wrong.
			Client.LinkSideBarToInterface(5065, 8);
			Client.LinkSideBarToInterface(5715, 9);
			Client.LinkSideBarToInterface(2449, 10);
			Client.LinkSideBarToInterface(4445, 11);
			Client.LinkSideBarToInterface(147, 12);
			Client.LinkSideBarToInterface(962, 13);
			Client.LinkSideBarToInterface(2423, 0);
			Client.LinkSideBarToInterface(5855, 0);
		}
	}
}
