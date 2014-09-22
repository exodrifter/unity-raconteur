using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyComment : RenPyLine
	{
		private string m_comment;
		public string Comment
		{
			get {
				return m_comment;
			}
		}

		public RenPyComment(ref RenPyScanner tokens) : base(RenPyLineType.COMMENT)
		{
			tokens.Seek("#");
			tokens.Next();
			m_comment = tokens.Seek("\n");
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "#" + m_comment;

			str += base.ToString();
			return str;
		}
	}
}
