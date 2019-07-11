using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reinterpret.Net;
using Rs317.Sharp;

namespace Rs317.Extended
{
	public sealed class ExtendedOpenTkClient : OpenTKClient
	{
		public ExtendedOpenTkClient(ClientConfiguration config, 
			OpenTKRsGraphicsContext graphicsObject, 
			IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext> 
				imageProducerFactory, IBufferFactory bufferFactory) 
			: base(config, graphicsObject, imageProducerFactory, bufferFactory)
		{

		}

		protected override int ReadPacketHeader(int currentAvailableBytes)
		{
			//3 byte header
			socket.read(inStream.buffer, 3);

			//2 byte length
			short payloadSize = inStream.buffer.Reinterpret<short>(0);
			packetSize = payloadSize;

			//1 byte opcode, for the client we don't preserve it in the stream since it doesn't use similar deserialization.
			packetOpcode = inStream.buffer[2] & 0xFF;

			currentAvailableBytes -= 3;
			return currentAvailableBytes;
		}
	}
}
