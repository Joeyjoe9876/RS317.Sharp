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

		public IImagePaintEventListener ImagePaintListener { get; }

		public OpenTKClient(ClientConfiguration config, OpenTKRsGraphicsContext graphicsObject, IImagePaintEventListener imagePaintListener) 
			: base(config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
			ImagePaintListener = imagePaintListener ?? throw new ArgumentNullException(nameof(imagePaintListener));
		}

		protected override IRSGraphicsProvider<OpenTKRsGraphicsContext> CreateGraphicsProvider()
		{
			return new OpenTKRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<OpenTKRsGraphicsContext> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			Console.WriteLine($"Created ImageProducer: {producerName}");

			OpenTKImageProducer imageProducer = new OpenTKImageProducer(xSize, ySize);
			ImagePaintListener.OnImageProducerCreated(imageProducer);

			return imageProducer;
		}
	}
}
