using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py pass statement.
	/// </summary>
	public class RenPyPass : RenPyStatement
	{
		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyPass(ref Scanner tokens) : base(RenPyStatementType.PASS)
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
