using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py label statement.
	/// </summary>
	public class RenPyLabel : RenPyStatement
	{
		private string m_name;
		public string Name
		{
			get {
				return m_name;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyLabel(ref Scanner tokens) : base(RenPyStatementType.LABEL)
		{
			tokens.Seek("label");
			tokens.Next();
			m_name = tokens.Seek(":").Trim();
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.Execution.PushStackFrame(NestedBlocks);
		}

		public override string ToDebugString()
		{
			string str = "label";
			str += " " + m_name + ":";
			return str;
		}
	}
}
