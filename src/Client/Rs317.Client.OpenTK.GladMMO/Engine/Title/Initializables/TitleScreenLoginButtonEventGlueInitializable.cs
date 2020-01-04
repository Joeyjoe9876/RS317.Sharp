using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Glader.Essentials;
using GladMMO.Client;
using Rs317.GladMMO;

namespace GladMMO
{
	[AdditionalRegisterationAs(typeof(ILoginButtonClickedEventSubscribable))]
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class TitleScreenLoginButtonEventGlueInitializable : IGameInitializable, ILoginButtonClickedEventSubscribable
	{
		private IUIButton LoginButton { get; }

		/// <inheritdoc />
		public event EventHandler OnLoginButtonClicked;

		/// <inheritdoc />
		public TitleScreenLoginButtonEventGlueInitializable([KeyFilter(ClientUIDependencyKey.LoginButton)] [NotNull] IUIButton loginButton)
		{
			LoginButton = loginButton ?? throw new ArgumentNullException(nameof(loginButton));
		}

		/// <inheritdoc />
		public Task OnGameInitialized()
		{
			LoginButton.IsInteractable = true;

			//When login button is pressed we should temporarily disable the interactivity of the login button.
			LoginButton.AddOnClickListener(() =>
			{
				//This kind of a hack, but login is being called twice right now
				//and I don't want to deal with or find out why.
				//if (LoginButton.IsInteractable == false)
				//	return;

				LoginButton.IsInteractable = false;

				//We should just dispatch on click.
				OnLoginButtonClicked?.Invoke(this, EventArgs.Empty);
			});

			return Task.CompletedTask;
		}
	}
}