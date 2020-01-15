using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using GladMMO;

namespace Rs317.GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class ZoneIdInitializable : IGameInitializable
	{
		private IZoneDataRepository ZoneDataRepository { get; }

		private IZoneDataService ZoneDataServiceClient { get; }

		public ZoneIdInitializable([JetBrains.Annotations.NotNull] IZoneDataRepository zoneDataRepository, [JetBrains.Annotations.NotNull] IZoneDataService zoneDataServiceClient)
		{
			ZoneDataRepository = zoneDataRepository ?? throw new ArgumentNullException(nameof(zoneDataRepository));
			ZoneDataServiceClient = zoneDataServiceClient ?? throw new ArgumentNullException(nameof(zoneDataServiceClient));
		}

		public async Task OnGameInitialized()
		{
			var anyZoneConnectionEndpointAsync = await ZoneDataServiceClient.GetAnyZoneConnectionEndpointAsync();
			ZoneDataRepository.UpdateZoneId(anyZoneConnectionEndpointAsync.Result.ZoneId);
		}
	}
}
