using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine forget macro deletes a variable.
	/// </summary>
	public class TwineForgetMacro : TwineMacro
	{
		/// <summary>
		/// The variable to delete.
		/// </summary>
		private string m_variable;
		
		public TwineForgetMacro(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("forget");
			tokens.Next();

			tokens.Seek("$");
			tokens.Next();
			m_variable = tokens.Next();

			tokens.Seek(">>");
			tokens.Next();
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			state.DeleteVariable(m_variable);
			return new List<TwineLine>();
		}
		
		protected override string ToDebugString()
		{
			string str = "forget ";
			str += "$" + m_variable;
			return str;
		}
	}
}
