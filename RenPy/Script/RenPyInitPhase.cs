using System.Collections.Generic;

using DPek.Raconteur.RenPy.Script;

using DictionaryList = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<DPek.Raconteur.RenPy.Script.RenPyStatement>>;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A collection of init blocks.
	/// </summary>
	public class RenPyInitPhase
	{
		/// <summary>
		/// A list of statements in this RenPyInitPhase.
		/// </summary>
		private DictionaryList m_statements;
		public DictionaryList Statements
		{
			get {
				return m_statements;
			}
		}

		/// <summary>
		/// Creates a new RenPyInitPhase.
		/// </summary>
		public RenPyInitPhase() {
			m_statements = new DictionaryList();
		}

		/// <summary>
		/// Adds a statement to the RenPyInitPhase.
		/// </summary>
		/// <param name="statement">
		/// The statement to add to the RenPyInitPhase.
		/// </param>
		/// <param name="priority">
		/// The priority of the statement.
		/// </param>
		public void AddStatement(int priority, RenPyStatement statement) {
			if(!m_statements.ContainsKey(priority)) {
				m_statements.Add(priority, new List<RenPyStatement>());
			}

			m_statements[priority].Add(statement);
		}

		/// <summary>
		/// Combines this init phase with the passed init phase.
		/// </summary>
		/// <param name="phase">
		/// The phase to add init statements from.
		/// </param>
		public void AddPhase(ref RenPyInitPhase phase)
		{
			foreach(var kvp in phase.m_statements) {
				foreach(var statement in kvp.Value) {
					AddStatement(kvp.Key, statement);
				}
			}
		}

		/// <summary>
		/// Returns a collection of priorities used in this init phase.
		/// </summary>
		/// <returns>
		/// A collection of priorities used in this init phase.
		/// </returns>
		public DictionaryList.KeyCollection GetPriorities() {
			return m_statements.Keys;
		}

		/// <summary>
		/// Returns a list of statements at the specified init priority.
		/// </summary>
		/// <returns>
		/// A list of statements at the specified init priority.
		/// </returns>
		/// <param name="priority">
		/// The priority of the statements to return.
		/// </param>
		public List<RenPyStatement> GetStatements(int priority) {
			return m_statements[priority];
		}
	}
}
