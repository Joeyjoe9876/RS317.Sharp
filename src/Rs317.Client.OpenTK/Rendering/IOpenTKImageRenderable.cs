using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Rs317.Sharp
{
	public interface IOpenTKImageRenderable
	{
		/// <summary>
		/// Indicates if the image is dirty, and requires re-rendering.
		/// </summary>
		bool isDirty { get; }

		/// <summary>
		/// The dangerous exposed image data pointer.
		/// </summary>
		IntPtr ImageDataPointer { get; }

		/// <summary>
		/// X being the width.
		/// Y being the height.
		/// 3rd component being the xOffset.
		/// 4th component being the yOffset.
		/// </summary>
		Rectangle ImageLocation { get; }

		/// <summary>
		/// The syncronization object.
		/// </summary>
		object SyncObject { get; }

		/// <summary>
		/// Consumes the set dirty bit.
		/// Indicating that it has been handled by a party.
		/// WARNING: You should lock around <see cref="SyncObject"/> the entire time from read to finish of the dirty bit.
		/// Otherwise you could miss it being set.
		/// </summary>
		void ConsumeDirty();

		/// <summary>
		/// The name of the image producer.
		/// </summary>
		string Name { get; }
	}
}
