using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py return statement.
	/// </summary>
	public class RenPyReturn : RenPyStatement
	{
		public RenPyReturn(ref RenPyScanner tokens)
			: base(RenPyStatementType.RETURN)
		{
			tokens.Seek("return");
			tokens.Next();
		}

		public override void Execute(RenPyState display)
		{
			display.Execution.PopStackFrame();
		}

		public override string ToString()
		{
			string str = "return";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
