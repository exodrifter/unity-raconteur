using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py init statement.
	/// </summary>
	public class RenPyInit : RenPyStatement
	{
		private int m_priority;
		public int Priority
		{
			get {
				return m_priority;
			}
		}

		public RenPyInit(ref Scanner tokens) : base(RenPyStatementType.INIT)
		{
			tokens.Seek("init");
			tokens.Next();

			string str = tokens.Seek(":").Trim();
			bool success = int.TryParse(str, out m_priority);
			if(!success) {
				m_priority = 0;
			}
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.Execution.PushStackFrame(NestedBlocks);
		}

		public override string ToDebugString()
		{
			string str = "init";
			str += " " + m_priority + ":";
			return str;
		}
	}
}
