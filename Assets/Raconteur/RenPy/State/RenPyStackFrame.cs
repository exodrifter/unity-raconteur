using System.Collections.Generic;

using DPek.Raconteur.Util;
using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the state of a Ren'Py stack frame.
	/// </summary>
	public class RenPyStackFrame
	{
		/// <summary>
		/// The blocks in this stack.
		/// </summary>
		private readonly List<RenPyBlock> m_blocks;
		public List<RenPyBlock> Blocks
		{
			get { return m_blocks; }
		}

		/// <summary>
		/// The index of the statement.
		/// </summary>
		private int m_statementIndex;

		/// <summary>
		/// The index of the current block.
		/// </summary>
		private int m_blockIndex;

		/// <summary>
		/// A dictionary of labels to the corresponding block and statement
		/// indices.
		/// </summary>
		private Dictionary<string, Duple<int, int>> m_labelIndices;

		/// <summary>
		/// Creates a new RenPyStackFrame with the passed list of RenPyBlocks.
		/// </summary>
		/// <param name="blocks"></param>
		public RenPyStackFrame(List<RenPyBlock> blocks)
		{
			m_blocks = blocks;
			m_statementIndex = -1;
			m_blockIndex = 0;

			m_labelIndices = new Dictionary<string, Duple<int, int>>();
			for (int i = 0; i < m_blocks.Count; ++i)
			{
				for (int j = 0; j < m_blocks[i].Statements.Count; ++j)
				{
					if (m_blocks[i][j] is RenPyLabel)
					{
						var name = (m_blocks[i][j] as RenPyLabel).Name;
						var index = new Duple<int, int>(i, j);
						m_labelIndices.Add(name, index);
					}
				}
			}
		}

		public void Reset()
		{
			m_statementIndex = -1;
			m_blockIndex = 0;
		}

		/// <summary>
		/// Moves the stack frame to the previous statement and returns it.
		/// </summary>
		/// <returns>
		/// The previous statement or null if there is no previous statement.
		/// </returns>
		public RenPyStatement PreviousStatement()
		{
			--m_statementIndex;

			// Check if we need to go to the previous block
			if (m_statementIndex < 0)
			{
				if (m_blockIndex > 0)
				{
					m_blockIndex--;
					int statements = m_blocks[m_blockIndex].Statements.Count;
					m_statementIndex = statements - 1;
				}
				else
				{
					Reset();
				}
			}

			var statement = m_blocks[m_blockIndex][m_statementIndex];
			return statement;
		}

		/// <summary>
		/// Moves the stack frame to the next statement, executes that
		/// statement, and returns that statement.
		/// </summary>
		/// <param name="state">
		/// The state to execute the statement with.
		/// </param>
		/// <returns>
		/// The next statement or null if there is no next statement.
		/// </returns>
		public RenPyStatement NextStatement(RenPyState state)
		{
			++m_statementIndex;
			if (m_blockIndex >= m_blocks.Count)
			{
				return null;
			}

			// Check if we need to go to the next block
			if (m_statementIndex >= m_blocks[m_blockIndex].Statements.Count)
			{
				m_statementIndex = 0;
				++m_blockIndex;
			}

			// Check if we are any more statements left
			if (m_blockIndex >= m_blocks.Count)
			{
				return null;
			}

			var statement = m_blocks[m_blockIndex][m_statementIndex];
			statement.Execute(state);

			return statement;
		}

		/// <summary>
		/// Determines whether the stack frame has the specified label.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance has the specified label; otherwise,
		/// <c>false</c>.
		/// </returns>
		/// <param name="label">
		/// The label to find in the stack frame.
		/// </param>
		public bool HasLabel(string label)
		{
			return m_labelIndices.ContainsKey(label);
		}

		/// <summary>
		/// Moves the stack frame to the statement right before the specified
		/// label.
		/// </summary>
		public void GoToLabel(string label)
		{
			Duple<int, int> index = m_labelIndices[label];
			m_blockIndex = index.First;
			m_statementIndex = index.Second - 1;
		}

		/// <summary>
		/// Returns the previous statement.
		/// </summary>
		/// <returns>
		/// The previous statement or null if there is no previous statement.
		/// </returns>
		public RenPyStatement GetPreviousStatement()
		{
			int blockIndex = m_blockIndex;
			int index = m_statementIndex - 1;

			// Check if we need to go to the previous block
			if (index < 0)
			{
				if (m_blockIndex > 0)
				{
					blockIndex--;
					index = m_blocks[blockIndex].Statements.Count - 1;
				}
				else
				{
					Reset();
				}
			}

			var statement = m_blocks[blockIndex][index];
			return statement;
		}
	}
}
