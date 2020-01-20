using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rs317.Sharp
{
	public class RsUnityClient : Client<UnityRsGraphics>
	{
		public UnityRsGraphics GraphicsObject { get; }

		public RsUnityClient(ClientConfiguration config, UnityRsGraphics graphicsObject, IRunnableStarter runnableStarterStrategy)
			: base(config, new DefaultBufferFactory(), runnableStarterStrategy)
		{
			if(config == null) throw new ArgumentNullException(nameof(config));
			GraphicsObject = graphicsObject ?? throw new ArgumentNullException(nameof(graphicsObject));
		}

		protected override IRSGraphicsProvider<UnityRsGraphics> CreateGraphicsProvider()
		{
			return new UnityRsGraphicsProvider(GraphicsObject);
		}

		protected override BaseRsImageProducer<UnityRsGraphics> CreateNewImageProducer(int xSize, int ySize, string producerName)
		{
			return new UnityRsImageProducer(xSize, ySize, producerName, gameGraphics);
		}
	}
}