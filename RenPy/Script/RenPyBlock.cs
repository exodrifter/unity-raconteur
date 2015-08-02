using UnityEngine;
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
		public List<RenPyStatement> Statements
		{
			get {
				return m_statements;
			}
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

		/// <summary>
		/// Creates a new RenPyBlock containing the passed statements.
		/// </summary>
		/// <param name="statements">
		/// The statements to store in the RenPyBlock.
		/// </param>
		public RenPyBlock(List<RenPyStatement> statements)
		{
			m_statements = statements;
		}

		/// <summary>
		/// Removes the statement at the specified index.
		/// </summary>
		/// <param name="index">
		/// The index of the statement to remove.
		/// </param>
		public void RemoveAt(int index)
		{
			m_statements.RemoveAt(index);
		}
	}
}
