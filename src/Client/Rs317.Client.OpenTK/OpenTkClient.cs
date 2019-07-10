using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class OpenTKClient : Client<OpenTKRsGraphicsContext>
	{
		public OpenTKRsGraphicsContext GraphicsObject { get; }

		private IFactoryCreateable<ImageProducerFactoryCreationContext, OpenTKImageProducer> ImageProducerFactory { get; }

		public OpenTKClient(ClientConfiguration config, OpenTKRsGraphicsContext graphicsObject, IFactoryCreateable<ImageProducerFactoryCreationContext, OpenTKImageProducer> imageProducerFactory) 
			: base(config, new DefaultBufferFactory())
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
			ImageProducerFactory = imageProducerFactory ?? throw new ArgumentNullException(nameof(imageProducerFactory));
		}

		protected override IRSGraphicsProvider<OpenTKRsGraphicsContext> CreateGraphicsProvider()
		{
			return new OpenTKRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<OpenTKRsGraphicsContext> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			Console.WriteLine($"Created ImageProducer: {producerName}");

			return ImageProducerFactory.Create(new ImageProducerFactoryCreationContext(xSize, ySize, producerName));
		}
	}
}
