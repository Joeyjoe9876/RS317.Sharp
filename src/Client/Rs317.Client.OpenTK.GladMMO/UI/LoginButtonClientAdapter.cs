using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Glader.Essentials;
using Rs317.Sharp;

namespace Rs317.GladMMO
{
	public sealed class LoginButtonClientAdapter : IUIButton, IDisposable
	{
		private GladMMOOpenTkClient Client { get; }

		private Action OnClicked;

		public LoginButtonClientAdapter(GladMMOOpenTkClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			Client.OnLoginButtonClickedEvent += OnLoginButtonClicked;
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
			Client.OnLoginButtonClickedEvent -= OnLoginButtonClicked;
		}
	}
}
