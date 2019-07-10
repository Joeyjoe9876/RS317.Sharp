using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IImagePaintEventPublisher
	{
		event EventHandler<IOpenTKImageRenderable> OnImageRenderableCreated;
	}
}
