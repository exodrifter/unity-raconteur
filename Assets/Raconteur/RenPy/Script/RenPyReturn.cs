using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

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
		
		public override void Parse(ref Scanner tokens)
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
