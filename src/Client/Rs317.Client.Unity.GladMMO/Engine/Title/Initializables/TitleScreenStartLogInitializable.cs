using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Common.Logging;
using Glader.Essentials;
using GladMMO.Client;
using Rs317.GladMMO;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class TitleScreenStartLogInitializable : IGameInitializable
	{
		private ILog Logger { get; }

		public TitleScreenStartLogInitializable([JetBrains.Annotations.NotNull] ILog logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc />
		public Task OnGameInitialized()
		{
			if(Logger.IsErrorEnabled)
				Logger.Error($"Started Titlescreen.");

			return Task.CompletedTask;
		}
	}
}