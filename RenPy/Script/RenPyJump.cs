using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py jump statement.
	/// </summary>
	public class RenPyJump : RenPyStatement
	{
		private string m_target;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyJump(ref Scanner tokens) : base(RenPyStatementType.JUMP)
		{
			tokens.Seek("jump");
			tokens.Next();
			m_target = tokens.Seek("\n").Trim();
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.Execution.GoToLabel(m_target);
		}

		public override string ToDebugString()
		{
			string str = "jump";
			str += " " + m_target;
			return str;
		}
	}
}
