using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Common.Logging;
using GladMMO;
using GladNet;

namespace Rs317.Sharp
{
	public sealed class WebGLNetworkClientModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Test>()
				.As<IPeerPayloadSendService<GameClientPacketPayload>>()
				.As<IConnectionService>()
				.As<IManagedNetworkClient<GameClientPacketPayload, GameServerPacketPayload>>()
				.SingleInstance();

			builder.RegisterType<DefaultMessageContextFactory>()
				.As<IPeerMessageContextFactory>()
				.SingleInstance();

			//Now, with the new design we also have to register the game client itself
			builder.RegisterType<GameNetworkClient>()
				.AsImplementedInterfaces()
				.SingleInstance();
		}
	}

	public class Test : INetworkClientManager<GameServerPacketPayload, GameClientPacketPayload>, IPeerPayloadSendService<GameClientPacketPayload>, IConnectionService, IManagedNetworkClient<GameClientPacketPayload, GameServerPacketPayload>
	{
		public bool isConnected { get; }

		private RsUnityClient Client { get; }

		public Test([JetBrains.Annotations.NotNull] RsUnityClient client)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task StartHandlingNetworkClient(IManagedNetworkClient<GameClientPacketPayload, GameServerPacketPayload> client)
		{
			isNetworkHandling = true;
		}

		public async Task StopHandlingNetworkClient()
		{
			isNetworkHandling = false;
		}

		public bool isNetworkHandling { get; private set; }

		async Task<SendResult> IPeerPayloadSendService<GameClientPacketPayload>.SendMessage<TPayloadType>(TPayloadType payload, DeliveryMethod method = DeliveryMethod.ReliableOrdered)
		{
			throw new NotImplementedException();
		}

		async Task<SendResult> IPeerPayloadSendService<GameClientPacketPayload>.SendMessageImmediately<TPayloadType>(TPayloadType payload, DeliveryMethod method = DeliveryMethod.ReliableOrdered)
		{
			throw new NotImplementedException();
		}

		public async Task DisconnectAsync(int delay)
		{
			Client.logout();
		}

		public async Task<bool> ConnectAsync(string ip, int port)
		{
			await Client.ConnectToGameServer();
			return true;
		}

		public async Task<NetworkIncomingMessage<GameServerPacketPayload>> ReadMessageAsync(CancellationToken token)
		{
			throw new NotImplementedException();
		}

		public async Task<TResponseType> InterceptPayload<TResponseType>(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		public async Task WriteAsync(byte[] bytes, int offset, int count)
		{
			throw new NotImplementedException();
		}
	}
}
