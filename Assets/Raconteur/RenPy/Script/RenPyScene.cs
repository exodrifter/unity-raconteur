using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyScene : RenPyLine
	{
		private string m_imageVarName;

		public RenPyScene(ref RenPyScanner tokens) : base(RenPyLineType.SCENE)
		{
			tokens.Seek("scene");
			tokens.Next();

			m_imageVarName = tokens.Seek(new string[] {"\n", "with"}).Trim();

			// Check if there is a "with" token next and ignore it
			if (tokens.PeekIgnoreWhitespace(true, true, true) == "with") {
				tokens.Seek("with");
				tokens.Seek("\n"); // Ignore the argument for with
				tokens.Next();
			}
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "scene";
			str += " \"" + m_imageVarName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
