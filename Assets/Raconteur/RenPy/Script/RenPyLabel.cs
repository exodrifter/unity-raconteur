using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py label statement.
	/// </summary>
	public class RenPyLabel : RenPyStatement
	{
		[SerializeField]
		private string m_name;
		public string Name
		{
			get {
				return m_name;
			}
		}

		public RenPyLabel() : base(RenPyStatementType.LABEL)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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
