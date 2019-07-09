using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace Rs317.Sharp
{
	public sealed class HookableVariableValueChangedEventArgs<TVariableType> : EventArgs
	{
		/// <summary>
		/// The current/old value.
		/// </summary>
		public TVariableType CurrentValue { get; }

		/// <summary>
		/// The value being changed to.
		/// </summary>
		public TVariableType NewValue { get; }

		public HookableVariableValueChangedEventArgs(TVariableType currentValue, TVariableType newValue)
		{
			CurrentValue = currentValue;
			NewValue = newValue;
		}
	}

	public struct HookableVariable<TVariableType>
	{
		/// <summary>
		/// Fired when the hookable variable's value changes.
		/// </summary>
		public EventHandler<HookableVariableValueChangedEventArgs<TVariableType>> OnVariableValueChanged { get; }

		public TVariableType VariableValue { get; private set; }

		//For performance reasons I choose not to lock.
		private readonly object SyncObj;

		public HookableVariable(TVariableType variableValue)
		{
			VariableValue = variableValue;
			OnVariableValueChanged = null;
			SyncObj = new object();
		}

		/// <summary>
		/// Implict cast consstructor to map between the hookable value to the
		/// <typeparamref name="TVariableType"/> value.
		/// </summary>
		/// <param name="hookable">The hookable.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator TVariableType(HookableVariable<TVariableType> hookable)
		{
			return hookable.VariableValue;
		}

		internal void Update(TVariableType value)
		{
			if(!Equals(value, VariableValue))
				OnVariableValueChanged?.Invoke(this, new HookableVariableValueChangedEventArgs<TVariableType>(VariableValue, value));

			VariableValue = value;
		}
	}
}
