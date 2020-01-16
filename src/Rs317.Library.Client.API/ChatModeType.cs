using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	/*if(publicChatMode == 0)
		fontPlain.drawCentredTextWithPotentialShadow("On", 55, 41, 0x00FF00, true);
	if(publicChatMode == 1)
		fontPlain.drawCentredTextWithPotentialShadow("Friends", 55, 41, 0xFFFF00, true);
	if(publicChatMode == 2)
		fontPlain.drawCentredTextWithPotentialShadow("Off", 55, 41, 0xFF0000, true);
	if(publicChatMode == 3)*/

	public enum ChatModeType : byte
	{
		On = 0,

		FriendsOnly = 1,

		Off = 2,
	}
}