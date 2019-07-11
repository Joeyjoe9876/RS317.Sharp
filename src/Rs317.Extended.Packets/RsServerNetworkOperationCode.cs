using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Extended
{
	/// <summary>
	/// Operation codes for the RS317 packets.
	/// </summary>
	public enum RsServerNetworkOperationCode : byte
	{
		/// <summary>
		/// Operation code of the packet that will set the an interface
		/// that is walkable.
		/// </summary>
		SetClientWalkableInterface = 208,

		SetPlayerRightClickOptions = 104,

		SetPlayerNetworkStatus = 249,

		SetChatModeStatus = 206,

		SetSkillExperience = 134,

		CameraReset = 107,

		ResetInterfaceButtonState = 68,

		LinkTabsToInterface = 71,

		WelcomeMessage = 176,

		SetRegion = 73,
	}
}
