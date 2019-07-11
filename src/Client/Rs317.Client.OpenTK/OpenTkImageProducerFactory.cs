using System;
using System.Collections.Generic;
using System.Text;
using Rs317.Sharp;

namespace Rs317.Sharp
{
	public sealed class OpenTkImageProducerFactory : IImagePaintEventPublisher, IFactoryCreateable<OpenTKImageProducer, ImageProducerFactoryCreationContext>
	{
		/// <summary>
		/// Event that should be invoked when a renderable is made.
		/// </summary>
		public event EventHandler<IOpenTKImageRenderable> OnImageRenderableCreated;

		public OpenTKImageProducer Create(ImageProducerFactoryCreationContext context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));

			OpenTKImageProducer imageProducer = new OpenTKImageProducer(context.Width, context.Height, context.Name);

			//Dispatch the event.
			OnImageRenderableCreated?.Invoke(this, imageProducer);

			return imageProducer;
		}
	}
}
