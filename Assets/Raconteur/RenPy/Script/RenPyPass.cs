using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py pass statement.
	/// </summary>
	public class RenPyPass : RenPyStatement
	{
		public RenPyPass(ref RenPyScanner tokens)
			: base(RenPyStatementType.PASS)
		{
			tokens.Seek("pass");
			tokens.Next();
		}
		
		public override void Execute(RenPyState state)
		{
			// Do nothing
		}
		
		public override string ToString()
		{
			string str = "pass";
			
			str += "\n" + base.ToString();
			return str;
		}
	}
}
