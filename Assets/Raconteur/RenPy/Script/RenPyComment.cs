using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyComment : RenPyStatement
	{
		private string m_comment;
		public string Comment
		{
			get {
				return m_comment;
			}
		}

		public RenPyComment(ref RenPyScanner tokens) : base(RenPyStatementType.COMMENT)
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
