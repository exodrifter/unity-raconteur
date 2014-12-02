using DPek.Raconteur.RenPy.State;
using System;

namespace DPek.Raconteur.RenPy.Script
{
	public class ValueString : Value
	{
		protected string m_str;

		public ValueString(string str)
		{
			UnityEngine.Debug.Log("Creating string with \"" + str + "\"");
			m_str = str;
		}
		
		public override Value GetValue(RenPyState state)
		{
			return new ValueString(m_str);
		}
		
		public override object GetRawValue(RenPyState state)
		{
			return m_str;
		}
		
		public override void SetValue(RenPyState state, Value value)
		{
			string msg = "Cannot assign a value to a number";
			throw new InvalidOperationException(msg);
		}

		public override string AsString(RenPyState state)
		{
			return m_str;
		}
		
		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override string ToString ()
		{
			return m_str;
		}
	}
}