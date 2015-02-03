using UnityEngine;
using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py menu choice statement
	/// </summary>
	public class RenPyMenuChoice : RenPyStatement
	{
		private string m_text;
		public string Text
		{
			get {
				return m_text;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyMenuChoice(ref Scanner tokens) : base(RenPyStatementType.MENU_CHOICE)
		{
			string[] quotes = new string[] {"\"","'"};
			string endQuote;

			tokens.Seek(quotes, out endQuote);
			tokens.Next();

			m_text = tokens.Seek(endQuote);
			tokens.Seek(":");
			tokens.Next();
		}
		
		public override void Execute(RenPyState state)
		{
			state.Execution.PushStackFrame(NestedBlocks);
		}
		
		public override string ToDebugString()
		{
			string str = "\"" + m_text + "\":";
			return str;
		}
	}
}
