using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/// <summary>
	/// Enumeration of known response codes to the connection initialization.
	/// </summary>
	public enum ConnectionInitializationResponseCode : sbyte
	{
		Success = 0,

		/// <summary>
		/// Waits for 2000ms and tries again.
		/// </summary>
		TryAgainLater = 1,

		/// <summary>
		/// Client made a successful login.
		/// </summary>
		SuccessfulLogin = 2,

		/// <summary>
		/// Invalid username or password.
		/// </summary>
		InvalidCredentials = 3,

		/// <summary>
		/// Your account has been disabled. Please check your message-center for details.
		/// </summary>
		Banned = 4,

		/// <summary>
		/// Your account is already logged in. Try again in 60 secs...
		/// </summary>
		AlreadyLoggedIn = 5,

		/// <summary>
		/// RuneScape has been updated! Please reload this page
		/// </summary>
		ClientOutOfDate = 6,

		/// <summary>
		/// This world is full. Please use a different world.
		/// </summary>
		WorldFull = 7,

		/// <summary>
		/// Unable to connect. Login server offline
		/// </summary>
		UnknownFailure = 8,

		/// <summary>
		/// Login limit exceeded. Too many connections from your address
		/// </summary>
		TooManyConcurrentConnections = 9,

		/// <summary>
		/// Unable to connect. Bad session id
		/// </summary>
		BadSessionId = 10,

		/// <summary>
		/// Login server rejected session. Please try again
		/// </summary>
		Rejected = 11,

		/// <summary>
		/// You need a members account to login to this world. Please subscribe, or use a different world
		/// </summary>
		RequiresMembership = 12,

		/// <summary>
		/// Could not complete login. Please try using a different world.
		/// </summary>
		IncompleteLogin = 13,

		/// <summary>
		/// The server is being updated. Please wait 1 minute and try again
		/// </summary>
		ServerMaintence = 14,

		//TODO: 15
		UnknownSuccessState = 15,

		/// <summary>
		/// Login attempts exceeded. Please wait 1 minute and try again.
		/// </summary>
		LoginsThrottled = 16,

		/// <summary>
		/// You are standing in a members-only area. To play on this world move to a free area first
		/// </summary>
		InvalidLoginArea = 17,

		/// <summary>
		/// Invalid loginserver requested. Please try using a different world.
		/// </summary>
		TryAnotherServer = 20,

		/// <summary>
		/// You have only just left another world. Your profile will be transferred in: (number) seconds
		/// </summary>
		SessionExistsOnAnotherServer = 21,
	}
}