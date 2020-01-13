using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Rs317.Sharp
{
	public sealed class MobileTitlescreenKeyboardInputHandler : MonoBehaviour
	{
		private IUITitleScreenStateHookable TileScreenHookable { get; set; }

		private bool ShouldBeShowingKeyboard = false;

		private TouchScreenKeyboard TouchKeyboardReference = null;

		private bool NeedsKeyboardSwitch = false;

		private object SyncObj = new object();

		public void Initialize([NotNull] IUITitleScreenStateHookable tileScreenHookable)
		{
			TileScreenHookable = tileScreenHookable ?? throw new ArgumentNullException(nameof(tileScreenHookable));

			TileScreenHookable.LoginScreenFocus.OnVariableValueChanged += LoginScreenFocusOnVariableValueChanged;
			TileScreenHookable.LoginScreenState.OnVariableValueChanged += LoginScreenStateOnVariableValueChanged;
		}

		private void LoginScreenStateOnVariableValueChanged(object sender, HookableVariableValueChangedEventArgs<TitleScreenState> e)
		{
			lock (SyncObj)
			{
				//If they go back to title screen or if the new userbox is entered
				//we shouldn't show a keyboard.
				if(e.CurrentValue == TitleScreenState.Default)
					ShouldBeShowingKeyboard = false;

				if(e.CurrentValue == TitleScreenState.NewUserBox)
					ShouldBeShowingKeyboard = false;

				if(e.CurrentValue == TitleScreenState.LoginBox)
					ShouldBeShowingKeyboard = true;
			}
		}

		private void LoginScreenFocusOnVariableValueChanged(object sender, HookableVariableValueChangedEventArgs<TitleScreenUIElement> e)
		{
			lock (SyncObj)
			{
				if (TileScreenHookable.LoginScreenState.VariableValue != TitleScreenState.LoginBox)
				{
					ShouldBeShowingKeyboard = false;
				}
				else
				{
					//If we're already showing, we should siwtch then.
					if(ShouldBeShowingKeyboard && e.CurrentValue != e.NewValue) //don't need to switch if it's the same value.
						NeedsKeyboardSwitch = true;

					//Always want to show.
					ShouldBeShowingKeyboard = true;
				}
			}
		}

		void Update()
		{
			if (TileScreenHookable == null)
				return;

			lock (SyncObj)
			{
				//The below Input handling has race conditions in it but it's not worth solving them, it shouldn't happen.
				//Open keyboard input if it's needed
				if (ShouldBeShowingKeyboard && TouchKeyboardReference == null)
					TouchKeyboardReference = CreateNewTouchKeyboard();

				if (ShouldBeShowingKeyboard && NeedsKeyboardSwitch && TouchKeyboardReference != null)
				{
					TouchKeyboardReference.active = false;
					TouchKeyboardReference = null;

					TouchKeyboardReference = CreateNewTouchKeyboard();
					NeedsKeyboardSwitch = false;
				}

				if (!ShouldBeShowingKeyboard && TouchKeyboardReference != null)
				{
					TouchKeyboardReference.active = false;
					TouchKeyboardReference = null;
				}

				//Set the login screen text target.
				if(TouchKeyboardReference != null)
					SetCurrentTextField(TouchKeyboardReference.text);

				//If it's inactive let's remove it.
				if (TouchKeyboardReference != null && !TouchKeyboardReference.active)
				{
					ShouldBeShowingKeyboard = false;
					TouchKeyboardReference = null;
				}
			}
		}

		private void SetCurrentTextField(string text)
		{
			switch (TileScreenHookable.LoginScreenFocus.VariableValue)
			{
				case TitleScreenUIElement.Default:
					TileScreenHookable.EnteredUsername = text;
					break;
				case TitleScreenUIElement.PasswordInputField:
					TileScreenHookable.EnteredPassword = text;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private TouchScreenKeyboard CreateNewTouchKeyboard()
		{
			return TouchScreenKeyboard.Open(GetCurrentKeyboardString(), TouchScreenKeyboardType.ASCIICapable, false, false, IsSecureInputRequired(), false, "", ComputeInputLimit());
		}

		private int ComputeInputLimit()
		{
			switch(TileScreenHookable.LoginScreenFocus.VariableValue)
			{
				case TitleScreenUIElement.Default:
					return UIInputConstants.MAX_USERNAME_INPUT_LIMIT;
				case TitleScreenUIElement.PasswordInputField:
					return UIInputConstants.MAX_PASSWORD_INPUT_LIMIT;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private bool IsSecureInputRequired()
		{
			return TileScreenHookable.LoginScreenFocus == TitleScreenUIElement.PasswordInputField;
		}

		private string GetCurrentKeyboardString()
		{
			switch (TileScreenHookable.LoginScreenFocus.VariableValue)
			{
				case TitleScreenUIElement.Default:
					return TileScreenHookable.EnteredUsername;
				case TitleScreenUIElement.PasswordInputField:
					return TileScreenHookable.EnteredPassword;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
