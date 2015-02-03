using System;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.Util.Expressions
{
	public class ValueNumber : Value
	{
		/// <summary>
		/// The value of this number.
		/// </summary>
		protected object m_value;

		/// <summary>
		/// Creates a new number from the specified string.
		/// </summary>
		/// <param name="value">
		/// The string representing the value of this number
		/// </param>
		public ValueNumber(string value)
		{
			m_value = ParseNumber(value);
			if(m_value == null)
			{
				string msg = "Parameter is not a number";
				throw new ArgumentException(msg, "value");
			}
		}
		
		/// <summary>
		/// Creates a new number from the specified int.
		/// </summary>
		/// <param name="value">
		/// The value of this number as an int.
		/// </param>
		public ValueNumber(int? value)
		{
			m_value = value;
		}
		
		/// <summary>
		/// Creates a new number from the specified float.
		/// </summary>
		/// <param name="value">
		/// The value of this number as an float.
		/// </param>
		public ValueNumber(float? value)
		{
			m_value = value;
		}
		
		/// <summary>
		/// Creates a new number from the specified double.
		/// </summary>
		/// <param name="value">
		/// The value of this number as an double.
		/// </param>
		public ValueNumber(double? value)
		{
			m_value = value;
		}

		/// <summary>
		/// Returns the value of this number.
		/// </summary>
		/// <returns>
		/// The value of this number.
		/// </returns>
		public override Value GetValue(RenPyState state)
		{
			if(m_value != null)
			{
				return new ValueNumber(this.AsString(state));
			}
			return null;
		}

		public override object GetRawValue(RenPyState state)
		{
			return m_value;
		}

		/// <summary>
		/// Setting the value of a number is not legal. For example, in C#, you
		/// may not do the following operation: "1 = 2;". In the same way,
		/// assigning a value to a number here is not legal.
		/// </summary>
		/// <param name="value">
		/// The value to set this number to.
		/// </param>
		public override void SetValue(RenPyState state, Value value)
		{
			string msg = "Cannot assign a value to a number";
			throw new InvalidOperationException(msg);
		}

		public override string AsString(RenPyState state) {
			return m_value.ToString();
		}
		
		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override string ToString ()
		{
			return m_value.ToString();
		}
	}
}