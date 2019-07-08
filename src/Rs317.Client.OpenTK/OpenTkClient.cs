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

		public OpenTKClient(ClientConfiguration config, OpenTKRsGraphicsContext graphicsObject) 
			: base(config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
		}

		protected override IRSGraphicsProvider<OpenTKRsGraphicsContext> CreateGraphicsProvider()
		{
			return new OpenTKRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<OpenTKRsGraphicsContext> CreateNewImageProducer(int xSize, int ySize)
		{
			return new OpenTKImageProducer(xSize, ySize);
		}
	}
}
