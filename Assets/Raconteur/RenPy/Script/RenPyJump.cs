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
		[SerializeField]
		private string m_target;

		public RenPyJump() : base(RenPyStatementType.JUMP)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
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
