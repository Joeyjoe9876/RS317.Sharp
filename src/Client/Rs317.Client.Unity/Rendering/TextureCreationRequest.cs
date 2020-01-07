using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class TextureCreationRequest
	{
		public int Width { get; }

		public int Height { get; }

		public string Name { get; }

		private TaskCompletionSource<TextureCreationResult> completionSource = new TaskCompletionSource<TextureCreationResult>();

		public TextureCreationRequest(int width, int height, [NotNull] string name)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));

			Width = width;
			Height = height;
			Name = name;
		}

		public Task<TextureCreationResult> GetCreationAwaitable()
		{
			return completionSource.Task;
		}

		public void CompleteRequest([NotNull] Texture2D texture)
		{
			if (texture == null) throw new ArgumentNullException(nameof(texture));

			completionSource.SetResult(new TextureCreationResult(texture));
		}
	}
}
