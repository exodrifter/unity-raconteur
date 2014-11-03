using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py return statement.
	/// </summary>
	public class RenPyReturn : RenPyStatement
	{
		public RenPyReturn() : base(RenPyStatementType.RETURN)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("return");
			tokens.Next();
		}

		public override void Execute(RenPyState display)
		{
			display.Execution.PopStackFrame();
		}

		public override string ToDebugString()
		{
			string str = "return";
			return str;
		}
	}
}
