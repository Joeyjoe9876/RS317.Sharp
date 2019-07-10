using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rs317.Sharp
{
	public sealed class RsWinFormsClient : Client<Graphics>
	{
		public Graphics GraphicsObject { get; }

		public RsWinFormsClient(ClientConfiguration config, Graphics graphicsObject) 
			: base(config)
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
		}

		protected override IRSGraphicsProvider<Graphics> CreateGraphicsProvider()
		{
			return new SystemDrawingRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<Graphics> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			return new SystemDrawingRsImageProducer(xSize, ySize, producerName);
		}
	}
}
