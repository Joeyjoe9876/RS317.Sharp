using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GladNet;

namespace Rs317.Extended
{
	public sealed class MockedPayloadInterceptorService : IPeerRequestSendService<BaseGameServerPayload>
	{
		/// <inheritdoc />
		public Task<TResponseType> SendRequestAsync<TResponseType>(BaseGameServerPayload request, DeliveryMethod method = DeliveryMethod.ReliableOrdered, CancellationToken cancellationToken = new CancellationToken())
		{
			throw new NotSupportedException($"Servers do not support {nameof(IPeerRequestSendService<BaseGameServerPayload>)}");
		}
	}
}