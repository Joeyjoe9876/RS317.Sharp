using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class Unity2DClient : Client<Graphics>
	{
		public Graphics GraphicsObject { get; }

		public Unity2DClient(ClientConfiguration config, Graphics graphicsObject) 
			: base(config, new DefaultBufferFactory())
		{
			if (config == null) throw new ArgumentNullException(nameof(config));
			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
		}

		protected override IRSGraphicsProvider<Graphics> CreateGraphicsProvider()
		{
			return new Unity2DGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<Graphics> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			return new UnityTextureImageProducer(xSize, ySize, producerName);
		}
	}
}
