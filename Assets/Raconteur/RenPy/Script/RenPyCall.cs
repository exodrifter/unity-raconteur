using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyCall : RenPyStatement
	{
		private string m_label;

		public RenPyCall(ref RenPyScanner tokens)
			: base(RenPyStatementType.CALL)
		{
			tokens.Seek("call");
			tokens.Next();
			tokens.SkipWhitespace();

			// Get the label that we want to call
			m_label = tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			var frame = state.Execution.InitialStackFrame.Blocks;
			state.Execution.PushStackFrame(frame);
			state.Execution.GoToLabel(m_label);
		}
		
		public override string ToString()
		{
			string str = "call";
			str += " " + m_label;
			
			str += "\n" + base.ToString();
			return str;
		}
	}
}
