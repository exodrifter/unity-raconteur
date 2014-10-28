using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

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

		public RenPyMenuChoice(ref RenPyScanner tokens)
			: base(RenPyStatementType.MENU_CHOICE)
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
		
		public override string ToString()
		{
			string str = "\"" + m_text + "\":";
			
			str += "\n" + base.ToString();
			return str;
		}
	}
}
