using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using AOT;

namespace Rs317.Sharp
{
	/// <summary>
	/// Class providing static access methods to work with JSLIB WebSocket or WebSocketSharp interface
	/// </summary>
	public class WebGLWebSocketFactory : IRsSocketFactory
	{
		/* Map of websocket instances */
		public static Dictionary<Int32, WebGLWebSocket> instances = new Dictionary<Int32, WebGLWebSocket>();

		/* Delegates */
		public delegate void OnOpenCallback(int instanceId);

		public delegate void OnMessageCallback(int instanceId, System.IntPtr msgPtr, int msgSize);

		public delegate void OnErrorCallback(int instanceId, System.IntPtr errorPtr);

		public delegate void OnCloseCallback(int instanceId, int closeCode);

		/* WebSocket JSLIB callback setters and other functions */
		[DllImport("__Internal")]
		public static extern int WebSocketAllocate(string url);

		[DllImport("__Internal")]
		public static extern void WebSocketFree(int instanceId);

		[DllImport("__Internal")]
		public static extern void WebSocketSetOnOpen(OnOpenCallback callback);

		[DllImport("__Internal")]
		public static extern void WebSocketSetOnMessage(OnMessageCallback callback);

		[DllImport("__Internal")]
		public static extern void WebSocketSetOnError(OnErrorCallback callback);

		[DllImport("__Internal")]
		public static extern void WebSocketSetOnClose(OnCloseCallback callback);

		/* If callbacks was initialized and set */
		public static bool isInitialized = false;

		/*
		 * Initialize WebSocket callbacks to JSLIB
		 */
		public static void Initialize()
		{
			WebSocketSetOnOpen(DelegateOnOpenEvent);
			WebSocketSetOnMessage(DelegateOnMessageEvent);
			WebSocketSetOnError(DelegateOnErrorEvent);
			WebSocketSetOnClose(DelegateOnCloseEvent);

			isInitialized = true;
		}

		/// <summary>
		/// Called when instance is destroyed (by destructor)
		/// Method removes instance from map and free it in JSLIB implementation
		/// </summary>
		/// <param name="instanceId">Instance identifier.</param>
		public static void HandleInstanceDestroy(int instanceId)
		{
			instances.Remove(instanceId);
			WebSocketFree(instanceId);
		}

		[MonoPInvokeCallback(typeof(OnOpenCallback))]
		public static void DelegateOnOpenEvent(int instanceId)
		{
			if (instances.TryGetValue(instanceId, out var instanceRef))
			{
				instanceRef.DelegateOnOpenEvent();
			}
		}

		[MonoPInvokeCallback(typeof(OnMessageCallback))]
		public static void DelegateOnMessageEvent(int instanceId, System.IntPtr msgPtr, int msgSize)
		{
			if (instances.TryGetValue(instanceId, out var instanceRef))
			{
				byte[] msg = new byte[msgSize];
				Marshal.Copy(msgPtr, msg, 0, msgSize);

				instanceRef.DelegateOnMessageEvent(msg);
			}
		}

		[MonoPInvokeCallback(typeof(OnErrorCallback))]
		public static void DelegateOnErrorEvent(int instanceId, System.IntPtr errorPtr)
		{
			if (instances.TryGetValue(instanceId, out var instanceRef))
			{
				string errorMsg = Marshal.PtrToStringAuto(errorPtr);
				instanceRef.DelegateOnErrorEvent(errorMsg);
			}
		}

		[MonoPInvokeCallback(typeof(OnCloseCallback))]
		public static void DelegateOnCloseEvent(int instanceId, int closeCode)
		{
			if (instances.TryGetValue(instanceId, out var instanceRef))
			{
				instanceRef.DelegateOnCloseEvent(closeCode);
			}
		}

		public IRsSocket Create(SocketCreationContext context)
		{
			string url = $"ws://{context.Endpoint}:{context.Port}";
			int instanceId = WebSocketAllocate(url);

			WebGLWebSocket socket = new WebGLWebSocket(url, instanceId);
			instances.Add(instanceId, socket);
			socket.Connect();

			//Now we make an adapter for the IRsSocket interface.
			return new WebGLRsSocketClientAdapter(socket);
		}
	}
}