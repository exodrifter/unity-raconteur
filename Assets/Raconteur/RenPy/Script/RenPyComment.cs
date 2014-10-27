using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py comment.
	/// </summary>
	public class RenPyComment : RenPyStatement
	{
		/// <summary>
		/// The contents of this comment.
		/// </summary>
		private string m_comment;
		public string Comment
		{
			get {
				return m_comment;
			}
		}

		public RenPyComment(ref RenPyScanner tokens)
			: base(RenPyStatementType.COMMENT)
		{
			tokens.Seek("#");
			tokens.Next();
			m_comment = tokens.Seek("\n");
		}

		public override void Execute(RenPyState state)
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
