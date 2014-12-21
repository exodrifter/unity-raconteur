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
		[SerializeField]
		private string m_comment;
		public string Comment
		{
			get {
				return m_comment;
			}
		}

		public RenPyComment() : base(RenPyStatementType.COMMENT)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
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
