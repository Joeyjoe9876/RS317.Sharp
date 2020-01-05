using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using GladMMO;

namespace Rs317.GladMMO
{
	[AdditionalRegisterationAs(typeof(IGeneralErrorEncounteredEventPublisher))]
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class StubGeneralErrorEventPublisher : IGeneralErrorEncounteredEventPublisher, IGameInitializable
	{
		public void PublishEvent(object sender, GeneralErrorEncounteredEventArgs eventArgs)
		{
			Console.WriteLine($"Error: {eventArgs.ErrorTitle} Message: {eventArgs.ErrorMessage}");
		}

		public Task OnGameInitialized()
		{
			return Task.CompletedTask;
		}
	}
}
