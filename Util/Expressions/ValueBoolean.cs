using System;

namespace DPek.Raconteur.Util.Expressions
{
	public class ValueBoolean : Value
	{
		protected bool m_value;
		
		public ValueBoolean(bool value)
		{
			m_value = value;
		}

		public override Value GetValue(StoryState state)
		{
			return new ValueBoolean(m_value);
		}

		public override object GetRawValue(StoryState state)
		{
			return m_value;
		}

		public override void SetValue(StoryState state, Value value)
		{
			string msg = "Cannot assign a value to a boolean";
			throw new InvalidOperationException(msg);
		}

		public override string AsString(StoryState state)
		{
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
			return "bool:" + m_value.ToString();
		}
	}
}
