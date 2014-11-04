using UnityEngine;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py menu choice statement
	/// </summary>
	public class RenPyMenuChoice : RenPyStatement
	{
		[SerializeField]
		private string m_text;
		public string Text
		{
			get {
				return m_text;
			}
		}

		public RenPyMenuChoice() : base(RenPyStatementType.MENU_CHOICE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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
