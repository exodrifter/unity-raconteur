using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents a statement in RenPy.
	/// </summary>
	public abstract class RenPyStatement
	{
		/// <summary>
		/// The type of this statement.
		/// </summary>
		private RenPyStatementType m_type;
		public RenPyStatementType Type
		{
			get {
				return m_type;
			}
		}

		/// <summary>
		/// The blocks that are nested under this statement, or null if there
		/// are none.
		/// </summary>
		private List<RenPyBlock> m_nestedBlocks;
		public List<RenPyBlock> NestedBlocks
		{
			get
			{
				return m_nestedBlocks;
			}
			set
			{
				m_nestedBlocks = value;
			}
		}

		/// <summary>
		/// Creates a new RenPyStatement of the specified type.
		/// </summary>
		/// <param name="type">
		/// The type of this RenPyStatement.
		/// </param>
		protected RenPyStatement(RenPyStatementType type)
		{
			this.m_type = type;
		}
		
		/// <summary>
		/// Executes the actions that this line takes.
		/// </summary>
		/// <param name="state">
		/// The state that this statement can modify.
		/// </param>
		public abstract void Execute(RenPyState state);

		/// <summary>
		/// Returns the statement as a debug string.
		/// </summary>
		/// <returns>
		/// The statement as a debug string.
		/// </returns>
		public abstract string ToDebugString();

		public sealed override string ToString()
		{
			return ToDebugString() + "\n" + this.GetType().FullName;
		}
	}
}
