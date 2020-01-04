using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using GladNet;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class OnConnectionEstablishedClaimSessionEventListener : BaseSingleEventListenerInitializable<INetworkConnectionEstablishedEventSubscribable>
	{
		private IPeerPayloadSendService<GameClientPacketPayload> SendService { get; }

		private IReadonlyAuthTokenRepository AuthTokenRepository { get; }

		private ILocalCharacterDataRepository CharacterDataRepository { get; }

		private ICharacterService CharacterService { get; }

		public OnConnectionEstablishedClaimSessionEventListener(INetworkConnectionEstablishedEventSubscribable subscriptionService,
			[NotNull] IPeerPayloadSendService<GameClientPacketPayload> sendService,
			[NotNull] IReadonlyAuthTokenRepository authTokenRepository,
			[NotNull] ILocalCharacterDataRepository characterDataRepository,
			[NotNull] ICharacterService characterService) 
			: base(subscriptionService)
		{
			SendService = sendService ?? throw new ArgumentNullException(nameof(sendService));
			AuthTokenRepository = authTokenRepository ?? throw new ArgumentNullException(nameof(authTokenRepository));
			CharacterDataRepository = characterDataRepository ?? throw new ArgumentNullException(nameof(characterDataRepository));
			CharacterService = characterService ?? throw new ArgumentNullException(nameof(characterService));
		}

		protected override void OnEventFired(object source, EventArgs args)
		{
			//Once connection to the instance server is established
			//we must attempt to claim out session on to actually fully enter.
			Task.Factory.StartNew(async () =>
			{
				CharacterListResponse listResponse = await CharacterService.GetCharacters()
					.ConfigureAwait(false);

				await CharacterService.TryEnterSession(listResponse.CharacterIds.First())
					.ConfigureAwait(false);

				CharacterDataRepository.UpdateCharacterId(listResponse.CharacterIds.First());

				//TODO: When it comes to community servers, we should not expose the sensitive JWT to them. We need a better way to deal with auth against untrusted instance servers
				await SendService.SendMessage(new ClientSessionClaimRequestPayload(AuthTokenRepository.RetrieveWithType(), listResponse.CharacterIds.First()))
					.ConfigureAwait(false);
			});
		}
	}
}
