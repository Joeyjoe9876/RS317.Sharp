using System;
using System.Collections.Generic;
using System.Text;

namespace Rs317.Sharp
{
	public interface IUITitleScreenStateHookable
	{
		HookableVariable<TitleScreenState> LoginScreenState { get; }

		HookableVariable<TitleScreenUIElement> LoginScreenFocus { get; }
	}
}
