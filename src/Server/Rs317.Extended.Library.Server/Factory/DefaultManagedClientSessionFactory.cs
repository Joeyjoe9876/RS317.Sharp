using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	//[AdditionalRegisterationAs(typeof(ISessionDisconnectionEventSubscribable))]
	//[AdditionalRegisterationAs(typeof(IFactoryCreatable<ManagedClientSession<GameServerPacketPayload, GameClientPacketPayload>, ManagedClientSessionCreationContext>))]
	//[AdditionalRegisterationAs(typeof(IManagedClientSessionFactory))]
	//[ServerSceneTypeCreate(ServerSceneType.Default)]
	public sealed class DefaultManagedClientSessionFactory : IManagedClientSessionFactory//, ISessionDisconnectionEventSubscribable, IGameInitializable
	{
		private ILog Logger { get; }

		private MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>> HandlerService { get; }

		/// <inheritdoc />
		public event EventHandler<SessionStatusChangeEventArgs> OnSessionDisconnection;

		/// <inheritdoc />
		public DefaultManagedClientSessionFactory(
			[NotNull] ILog logger,
			[NotNull] MessageHandlerService<BaseGameClientPayload, BaseGameServerPayload, IPeerSessionMessageContext<BaseGameServerPayload>> handlerService)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			HandlerService = handlerService ?? throw new ArgumentNullException(nameof(handlerService));
		}

		/// <inheritdoc />
		public ManagedClientSession<BaseGameServerPayload, BaseGameClientPayload> Create([NotNull] ManagedClientSessionCreationContext context)
		{
			if(context == null) throw new ArgumentNullException(nameof(context));

			if(Logger.IsDebugEnabled)
				Logger.Debug($"Creating new session. Details: {context.Details}");

			try
			{

				GameClientSession clientSession = new GameClientSession(context.Client, context.Details, HandlerService, Logger);

				clientSession.OnSessionDisconnection += (source, args) =>
				{
					OnSessionDisconnection?.Invoke(source, args);
					return Task.CompletedTask;
				};

				return clientSession;
			}
			catch(Exception e)
			{
				if(Logger.IsErrorEnabled)
					Logger.Error($"Failed to create Client: {context.Details} Error: {e.Message} \n\n Stack: {e.StackTrace}");

				throw;
			}
		}

		//TODO: This is a hack to get it into the scene.
		/// <inheritdoc />
		public Task OnGameInitialized()
		{
			return Task.CompletedTask;
		}
	}
}