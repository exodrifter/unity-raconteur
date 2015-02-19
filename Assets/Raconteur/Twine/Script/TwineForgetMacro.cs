using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwineForgetMacro : TwineLine
	{
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
			state.SetVariable(m_variable, null);
			return new List<TwineLine>();
		}
		
		public override string Print()
		{
			return null;
		}
		
		protected override string ToDebugString()
		{
			return m_variable;
		}
	}
}
