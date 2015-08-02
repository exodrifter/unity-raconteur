using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

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

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyComment(ref Scanner tokens)
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

		public override string ToDebugString()
		{
			string str = "#" + m_comment;
			return str;
		}
	}
}
