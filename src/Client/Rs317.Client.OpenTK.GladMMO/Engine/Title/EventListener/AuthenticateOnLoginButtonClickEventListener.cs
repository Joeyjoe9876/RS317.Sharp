using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Common.Logging;
using Glader.Essentials;
using GladMMO;
using Refit;

namespace Rs317.GladMMO
{
	[AdditionalRegisterationAs(typeof(IAuthenticationResultRecievedEventSubscribable))]
	[SceneTypeCreateGladMMO(GameSceneType.TitleScreen)]
	public sealed class AuthenticateOnLoginButtonClickEventListener : BaseSingleEventListenerInitializable<ILoginButtonClickedEventSubscribable>, IAuthenticationResultRecievedEventSubscribable
	{
		private IAuthenticationService AuthService { get; }

		private IUIText UserNameField { get; }

		private IUIText PasswordField { get; }

		private ILog Logger { get; }

		private IGameContextEventQueueable GameQueueable { get; }

		private IAuthTokenRepository AuthTokenRepository { get; }

		public event EventHandler<AuthenticationResultEventArgs> OnAuthenticationResultRecieved;

		public AuthenticateOnLoginButtonClickEventListener(ILoginButtonClickedEventSubscribable subscriptionService, 
			IAuthenticationService authService,
			[KeyFilter(ClientUIDependencyKey.UserName)] IUIText userNameField,
			[KeyFilter(ClientUIDependencyKey.Password)] IUIText passwordField, 
			ILog logger, 
			IGameContextEventQueueable gameQueueable,
			[NotNull] IAuthTokenRepository authTokenRepository) 
			: base(subscriptionService)
		{
			AuthService = authService ?? throw new ArgumentNullException(nameof(authService));
			UserNameField = userNameField;
			PasswordField = passwordField;
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			GameQueueable = gameQueueable ?? throw new ArgumentNullException(nameof(gameQueueable));
			AuthTokenRepository = authTokenRepository ?? throw new ArgumentNullException(nameof(authTokenRepository));
		}

		protected override void OnEventFired(object source, EventArgs args)
		{
			Console.WriteLine($"Auth: {UserNameField.Text}:{PasswordField.Text}");
			Task.Factory.StartNew(async () =>
			{
				PlayerAccountJWTModel jwtModel = null;

				//TODO: Validate username and password
				//We can't do error code supression with refit anymore, so we have to do this crap.
				try
				{
					jwtModel = await AuthService.TryAuthenticate(new AuthenticationRequestModel(UserNameField.Text, PasswordField.Text))
						.ConfigureAwait(false);
				}
				catch(ApiException e)
				{
					jwtModel = e.GetContentAs<PlayerAccountJWTModel>();

					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Auth Error: {e.Message}");
				}
				catch(Exception e)
				{
					if(Logger.IsErrorEnabled)
						Logger.Error($"Encountered Auth Error: {e.Message}\n\nStack: {e.StackTrace}");
				}
				finally
				{
					if(Logger.IsDebugEnabled)
						Logger.Debug($"Auth Response for User: {UserNameField.Text} Result: {jwtModel?.isTokenValid} OptionalError: {jwtModel?.Error} OptionalErrorDescription: {jwtModel?.ErrorDescription}");

					if (jwtModel != null && jwtModel.isTokenValid)
					{
						AuthTokenRepository.Update(jwtModel.AccessToken);

						GameQueueable.Enqueue(() =>
						{
							//Even if it's null, we should broadcast the event.
							OnAuthenticationResultRecieved?.Invoke(this, new AuthenticationResultEventArgs(jwtModel));
						});
					}
				}
			});
		}
	}
}
