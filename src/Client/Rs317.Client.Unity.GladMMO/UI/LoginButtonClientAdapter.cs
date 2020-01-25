using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using JetBrains.Annotations;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class LoginButtonClientAdapter : IUIButton, IDisposable
	{
		//Persisted so we can dispose
		[NotNull]
		private IRSClientLoginButtonPressedEventSubscribable LoginButtonPressedSubscribable { get; }

		private Action OnClicked;

		public LoginButtonClientAdapter([NotNull] IRSClientLoginButtonPressedEventSubscribable loginButtonPressedSubscribable)
		{
			LoginButtonPressedSubscribable = loginButtonPressedSubscribable ?? throw new ArgumentNullException(nameof(loginButtonPressedSubscribable));
			loginButtonPressedSubscribable.OnRunescapeLoginButtonPressed += OnLoginButtonClicked;
		}

		private void OnLoginButtonClicked(object sender, EventArgs e)
		{
			OnClicked?.Invoke();
		}

		public void AddOnClickListener(Action action)
		{
			OnClicked += action;
		}

		public void AddOnClickListenerAsync(Func<Task> action)
		{
			throw new NotImplementedException("TODO: WE haven't implemented this for RS yet.");
		}

		public void SimulateClick(bool eventsOnly)
		{
			OnClicked?.Invoke();
		}

		public bool IsInteractable { get; set; } = true;

		public void Dispose()
		{
			LoginButtonPressedSubscribable.OnRunescapeLoginButtonPressed -= OnLoginButtonClicked;
		}
	}
}
