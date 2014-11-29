using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class ValueString : Value
	{
		private string m_str;

		public ValueString(string str)
		{
			m_str = str;
		}
		
		public override object GetValue(RenPyState state)
		{
			return m_str;
		}
		
		public override void SetValue(RenPyState state, string value)
		{
			m_str = value;
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