using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents a statement in RenPy.
	/// </summary>
	[System.Serializable]
	public abstract class RenPyStatement : ScriptableObject
	{
		/// <summary>
		/// The type of this statement.
		/// </summary>
		[SerializeField]
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
		[SerializeField]
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
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="scanner">
		/// The scanner to use to initialize this statement.
		/// </param>
		public abstract void Parse(ref Scanner scanner);

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
