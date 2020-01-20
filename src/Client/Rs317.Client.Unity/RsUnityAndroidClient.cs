using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Rs317.Sharp
{
	public sealed class RsUnityAndroidClient : RsUnityClient
	{
		public RsUnityAndroidClient(ClientConfiguration config, UnityRsGraphics graphicsObject) 
			: base(config, graphicsObject, new DefaultRunnableStarterStrategy())
		{
			if (config == null) throw new ArgumentNullException(nameof(config));

			UnityEngine.GameObject mobileBehaviorObject = new UnityEngine.GameObject();

			var inputHandler = mobileBehaviorObject.AddComponent<MobileTitlescreenKeyboardInputHandler>();
			inputHandler.Initialize(this);
		}
	}
}
