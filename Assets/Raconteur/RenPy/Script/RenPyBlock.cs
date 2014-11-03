using UnityEngine;
using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A RenPyBlock is a collection of statements that belong to a block of
	/// code such as a label, an init block, an if statement, etc.
	/// </summary>
	[System.Serializable]
	public class RenPyBlock : ScriptableObject
	{
		/// <summary>
		/// A list of statements in this RenPyBlock.
		/// </summary>
		[SerializeField]
		private List<RenPyStatement> m_statements;
		public List<RenPyStatement> Statements
		{
			get {
				return m_statements;
			}
			set {
				m_statements = value;
			}
		}

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
