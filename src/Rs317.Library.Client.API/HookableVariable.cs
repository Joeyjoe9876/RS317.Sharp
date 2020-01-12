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

	public class HookableVariable<TVariableType>
	{
		/// <summary>
		/// Fired when the hookable variable's value changes.
		/// </summary>
		public event EventHandler<HookableVariableValueChangedEventArgs<TVariableType>> OnVariableValueChanged;

		public TVariableType VariableValue { get; private set; }

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Update(TVariableType value)
		{
			try
			{
				if (!Equals(value, VariableValue))
					OnVariableValueChanged?.Invoke(this, new HookableVariableValueChangedEventArgs<TVariableType>(VariableValue, value));
			}
			catch (Exception e)
			{
				throw;
			}
			finally
			{
				lock(SyncObj)
					VariableValue = value;
			}
		}
	}
}
