using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IImagePaintEventListener
	{
		void OnImageProducerCreated(IOpenTKImageRenderable imageProducer);
	}
}
