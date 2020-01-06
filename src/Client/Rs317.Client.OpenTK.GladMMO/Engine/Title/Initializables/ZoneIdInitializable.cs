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

		public ZoneIdInitializable([JetBrains.Annotations.NotNull] IZoneDataRepository zoneDataRepository)
		{
			ZoneDataRepository = zoneDataRepository ?? throw new ArgumentNullException(nameof(zoneDataRepository));
		}

		public Task OnGameInitialized()
		{
			ZoneDataRepository.UpdateZoneId(170);
			return Task.CompletedTask;
		}
	}
}
