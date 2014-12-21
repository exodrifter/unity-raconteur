using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py pass statement.
	/// </summary>
	public class RenPyPass : RenPyStatement
	{
		public RenPyPass() : base(RenPyStatementType.PASS)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
		{
			tokens.Seek("pass");
			tokens.Next();
		}
		
		public override void Execute(RenPyState state)
		{
			// Do nothing
		}
		
		public override string ToDebugString()
		{
			return "pass";
		}
	}
}
