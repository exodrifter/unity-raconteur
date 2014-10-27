using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A RenPyBlock is a collection of statements that belong to a block of
	/// code such as a label, an init block, an if statement, etc.
	/// </summary>
	public class RenPyBlock
	{
		/// <summary>
		/// A list of statements in this RenPyBlock.
		/// </summary>
		private List<RenPyStatement> m_statements;

		/// <summary>
		/// The number of statements in this RenPyBlock.
		/// </summary>
		public int StatementCount
		{
			get {
				return m_statements.Count;
			}
		}

		/// <summary>
		/// Creates a new RenPyBlock with the referenced list of statements.
		/// </summary>
		/// <param name="statements">
		/// A list of statements that belong to this RenPyBlock.
		/// </param>
		public RenPyBlock(List<RenPyStatement> statements)
		{
			m_statements = statements;
		}

		/// <summary>
		/// Operator overload for accessing statements in the RenPyBlock.
		/// </summary>
		/// <param name="i">
		/// The index of the statement to access.
		/// </param>
		/// <returns>
		/// The RenPyStatement at the specified index.
		/// </returns>
		public RenPyStatement this[int i]
		{
			get {
				return m_statements[i];
			}
		}
	}
}
