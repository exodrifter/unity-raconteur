using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py else statement.
	/// </summary>
	public class RenPyWindow : RenPyStatement
	{
		private bool m_show;
		public bool Show
		{
			get {
				return m_show;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyWindow(ref Scanner tokens) : base(RenPyStatementType.WINDOW)
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
