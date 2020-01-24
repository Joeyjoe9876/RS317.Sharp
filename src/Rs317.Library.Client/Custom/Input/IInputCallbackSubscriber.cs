using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IInputCallbackSubscriber
	{
		void mouseExited(object sender, EventArgs e);

		void mouseEntered(object sender, EventArgs e);

		void focusLost(object sender, EventArgs e);

		void focusGained(object sender, EventArgs e);

		void keyTyped(object sender, RsKeyEventArgs e);

		void keyReleased(object sender, RsKeyEventArgs e);

		void keyPressed(object sender, RsKeyEventArgs e);

		void mouseMoved(object sender, RsMousePositionChangeEventArgs e);

		void mouseReleased(object sender, RsMouseInputEventArgs e);

		void mouseDragged(object sender, RsMousePositionChangeEventArgs e);

		void mousePressed(object sender, RsMouseInputEventArgs e);

		void mouseWheelDragged(object sender, RsMousePositionChangeEventArgs e);

		void mouseWheelScroll(object sender, float scrollDelta);
	}
}
