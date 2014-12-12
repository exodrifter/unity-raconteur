using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py else statement.
	/// </summary>
	public class RenPyWindow : RenPyStatement
	{
		[SerializeField]
		private bool m_show;
		public bool Show
		{
			get {
				return m_show;
			}
		}

		public RenPyWindow() : base(RenPyStatementType.WINDOW)
		{
			// Nothing to do
		}

		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("window");
			tokens.Next();

			tokens.Seek(new string[] { "show", "hide" });
			string mode = tokens.Next();
			if (mode == "show") {
				m_show = true;
			}
			else if (mode == "hide") {
				m_show = false;
			}
			else {
				Debug.LogError("Could not parse window statement; invalid "
					+ "window mode \"" + mode + "\"");
			}

			// Ignore the rest of the line
			tokens.Seek("\n");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.Visual.WindowRequested = m_show;
		}

		public override string ToDebugString()
		{
			string str = "window ";
			str += m_show ? "show" : "hide";
			return str;
		}
	}
}
