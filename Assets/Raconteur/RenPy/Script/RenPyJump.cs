using UnityEngine;

using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyJump : RenPyStatement
	{
		private string m_target;

		public RenPyJump(ref RenPyScanner tokens) : base(RenPyStatementType.JUMP)
		{
			tokens.Seek("jump");
			tokens.Next();
			m_target = tokens.Seek("\n").Trim();
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			Static.Log("jump " + m_target);

			bool success = display.State.GoToLabel(display, m_target);
			if (!success) {
				Debug.LogError("Could not find the label \"" + m_target + "\"");
			}
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
