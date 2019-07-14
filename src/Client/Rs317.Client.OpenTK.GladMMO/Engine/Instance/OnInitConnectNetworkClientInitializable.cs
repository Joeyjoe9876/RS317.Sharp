using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Glader.Essentials;
using GladNet;

namespace GladMMO
{
	[SceneTypeCreateGladMMO(GameSceneType.InstanceServerScene)]
	public sealed class OnInitConnectNetworkClientInitializable : IGameInitializable
	{
		/// <summary>
		/// The managed network client that the Unity3D client is implemented on-top of.
		/// </summary>
		private IManagedNetworkClient<GameClientPacketPayload, GameServerPacketPayload> Client { get; }

		private ILog Logger { get; }

		public OnInitConnectNetworkClientInitializable(
			[NotNull] IManagedNetworkClient<GameClientPacketPayload, GameServerPacketPayload> client,
			[NotNull] ILog logger)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task OnGameInitialized()
		{
			if(Client.isConnected)
				throw new InvalidOperationException($"Encountered network client already initialized.");

			//"192.168.0.12"), 
			await Client.ConnectAsync("192.168.0.12", 5006);
		}
	}
}