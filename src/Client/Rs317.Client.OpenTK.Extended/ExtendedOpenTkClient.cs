using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
