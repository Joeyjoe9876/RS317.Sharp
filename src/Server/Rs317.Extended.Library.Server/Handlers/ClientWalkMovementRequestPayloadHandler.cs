using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using GladNet;
using JetBrains.Annotations;

namespace Rs317.Extended
{
	[ServerOperationType]
	public class ClientWalkMovementRequestPayloadHandler : BaseServerRequestHandler<ClientWalkMovementRequestPayload>
	{
		public ClientWalkMovementRequestPayloadHandler([NotNull] ILog logger) 
			: base(logger)
		{
		}

		public override async Task HandleMessage(IPeerSessionMessageContext<BaseGameServerPayload> context, ClientWalkMovementRequestPayload payload)
		{
			Console.WriteLine($"Recieved Path with Length: {payload.PathPoints.Length}");

			foreach (var v in payload.PathPoints)
				Console.WriteLine($"X: {v.X} Y: {v.Y}");

			//await context.PayloadSendService.SendMessage(new ServerUpdateLocalPlayerPayload(payload.PathPoints.Last()));
		}
	}
}
