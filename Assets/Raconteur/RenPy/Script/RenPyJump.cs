using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py jump statement.
	/// </summary>
	public class RenPyJump : RenPyStatement
	{
		private string m_target;

		public RenPyJump(ref RenPyScanner tokens)
			: base(RenPyStatementType.JUMP)
		{
			tokens.Seek("jump");
			tokens.Next();
			m_target = tokens.Seek("\n").Trim();
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			Static.Log("jump " + m_target);

			state.Execution.GoToLabel(m_target);
			state.Execution.NextStatement(state);
		}

		public override string ToString()
		{
			string str = "jump";
			str += " " + m_target;

			str += "\n" + base.ToString();
			return str;
		}
	}
}
