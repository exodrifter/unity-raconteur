using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A Variable represents a string in an expression that refers to a
	/// variable.
	/// </summary>
	public class ValueVariable : Value
	{
		/// <summary>
		/// The name of the variable to look up and modify.
		/// </summary>
		private string m_variable;

		public ValueVariable(string variable)
		{
			m_variable = variable;
		}

		public override object GetValue(RenPyState state)
		{
			string value = state.GetVariable(m_variable);
			object number = ParseNumber(value);
			if(number != null)
			{
				return number;
			}
			return value;
		}
		
		public override void SetValue(RenPyState state, string value)
		{
			state.SetVariable(m_variable, value);
		}
		
		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override string ToString ()
		{
			return m_variable;
		}
	}
}
