using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyShow : RenPyLine
	{
		private string m_characterVarName;

		public RenPyShow(ref RenPyScanner tokens) : base(RenPyLineType.SHOW)
		{
			tokens.Seek("show");
			tokens.Next();

			string[] arr = new string[] { "\n", "with" };
			m_characterVarName = tokens.Seek(arr).Trim();

			// Check if there is a "with" token next and ignore it
			tokens.SkipWhitespace(true, true, true);
			if (tokens.Peek() == "with") {
				tokens.Seek("\n");
			}
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "show";
			str += " \"" + m_characterVarName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
