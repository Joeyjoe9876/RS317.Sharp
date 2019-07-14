using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Autofac;
using Glader.Essentials;
using Rs317.GladMMO;
using Rs317.Sharp;

namespace Rs317.GladMMMO
{
	public sealed class GameManager : IGameContextEventQueueable, IGameServiceable
	{
		private ConcurrentQueue<Action> EventQueue { get; } = new ConcurrentQueue<Action>();

		public static IDisposable LifetimeDependencyScope { get; set; }

		//Hacky, but this is mutable when the game state changes.
		private IEnumerable<IGameTickable> CurrentGameTickables { get; set; } = Array.Empty<IGameTickable>();

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void OnLoginStateChanged(object sender, HookableVariableValueChangedEventArgs<bool> e)
		{
			LifetimeDependencyScope?.Dispose();
			EventQueue.Clear();

			Console.WriteLine($"LoginState Changed");

			//We're now logged in, time to create new tickables and initializables.
			if (e.NewValue)
			{
				//We're going into the game!
				var container = BuildInstanceServerContainer().BeginLifetimeScope();
				InitializeNewContainerContext(container);
			}
			else
			{
				//We're going BACK to the titlescreen.
				var container = BuildTitleScreenContainer().BeginLifetimeScope();
				InitializeNewContainerContext(container);
			}
		}

		private void InitializeNewContainerContext(ILifetimeScope container)
		{
			if (container == null) throw new ArgumentNullException(nameof(container));

			var gameInitializables = container.Resolve<IEnumerable<IGameInitializable>>();
			var gameStartables = container.Resolve<IEnumerable<IGameStartable>>();
			CurrentGameTickables = container.Resolve<IEnumerable<IGameTickable>>();
			EventQueue.Enqueue(() =>
			{
				foreach (var initializable in gameInitializables)
					initializable.OnGameInitialized().ConfigureAwait(false).GetAwaiter().GetResult();

				foreach(var startable in gameStartables)
					startable.Start().ConfigureAwait(false).GetAwaiter().GetResult();
			});

			LifetimeDependencyScope = container;
		}

		public void Enqueue(Action actionEvent)
		{
			EventQueue.Enqueue(actionEvent);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Service()
		{
			//Just do one a tick.
			if (EventQueue.Any())
			{
				if (EventQueue.TryDequeue(out var action))
					action?.Invoke();
			}

			foreach(var tickable in CurrentGameTickables)
				tickable.Tick();
		}

		public static IContainer BuildTitleScreenContainer()
		{
			//Autofac is needed to create the GladMMO scene.
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule<TitlescreenDependencyModule>();
			return builder.Build();
		}

		public static IContainer BuildInstanceServerContainer()
		{
			//Autofac is needed to create the GladMMO scene.
			ContainerBuilder builder = new ContainerBuilder();
			builder.RegisterModule<InstanceServerDependencyModule>();
			return builder.Build();
		}
	}
}
