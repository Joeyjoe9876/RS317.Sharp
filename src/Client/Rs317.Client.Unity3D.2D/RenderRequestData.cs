using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class RenderRequestData
	{
		public string Name { get; }

		public Rect Location { get; }

		public Action<IntPtr> OnTextureIntialCreation;

		public RenderRequestData([NotNull] string name, Rect location, Action<IntPtr> onTextureIntialCreation)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

			Name = name;
			Location = location;
			OnTextureIntialCreation = onTextureIntialCreation;
		}
	}
}
