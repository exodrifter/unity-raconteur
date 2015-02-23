namespace DPek.Raconteur.Util.Expressions
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
			m_variable = variable.Replace("$", "");
		}

		public override Value GetValue(StoryState state)
		{
			string value = state.GetVariable(m_variable);
			object number = ParseNumber(value);
			if(number != null)
			{
				return new ValueNumber(value);
			}
			return new ValueString(value);
		}

		public override object GetRawValue(StoryState state)
		{
			string value = state.GetVariable(m_variable);
			object number = ParseNumber(value);
			if(number != null)
			{
				return number;
			}
			return value;
		}

		public override void SetValue(StoryState state, Value value)
		{
			state.SetVariable(m_variable, value.AsString(state));
		}

		public override string AsString(StoryState state)
		{
			return state.GetVariable(m_variable);
		}
		
		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override string ToString ()
		{
			return "var:" + m_variable;
		}
	}
}
