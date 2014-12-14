using System.Collections.Generic;

using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy.State
{
	/// <summary>
	/// Stores the execution state of a Ren'Py script.
	/// </summary>
	public class RenPyExecutionState
	{
		#region Member Variables

		/// <summary>
		/// The initial stack frame.
		/// </summary>
		private readonly RenPyStackFrame m_initialStackFrame;
		public RenPyStackFrame InitialStackFrame
		{
			get {
				return m_initialStackFrame;
			}
		}

		/// <summary>
		/// The Ren'Py call stack.
		/// </summary>
		private readonly Stack<RenPyStackFrame> m_stack;

		/// <summary>
		/// The statement that the execution state is currently pointing to.
		/// </summary>
		private RenPyStatement m_currentStatement;
		public RenPyStatement CurrentStatement
		{
			get
			{
				return m_currentStatement;
			}
		}

		/// <summary>
		/// A boolean that indicates whether or not the execution has stack
		/// frames.
		/// </summary>
		/// <value>
		/// <c>true</c> if running; otherwise, <c>false</c>.
		/// </value>
		public bool Running {
			get {
				return m_stack.Count > 0;
			}
		}

		#endregion

		/// <summary>
		/// Creates a new RenPyDialogState with the passed script.
		/// </summary>
		/// <param name="lines">
		/// The Ren'Py script as an array of RenPyLine objects.
		/// <param>
		public RenPyExecutionState(ref List<RenPyBlock> blocks)
		{
			m_initialStackFrame = new RenPyStackFrame(blocks);

			m_stack = new Stack<RenPyStackFrame>();
			m_stack.Push(m_initialStackFrame);
		}

		/// <summary>
		/// Moves the state to the previous statement and returns that
		/// statement.
		/// </summary>
		/// <returns>
		/// The previous statement or null if there is no previous statement.
		/// </returns>
		public RenPyStatement PreviousStatement()
		{
			if(m_stack.Count > 0) {
				m_currentStatement = m_stack.Peek().PreviousStatement();
				return m_currentStatement;
			}
			return null;
		}

		/// <summary>
		/// Moves the state to the next statement, executes it, and returns it.
		/// </summary>
		/// <param name="state">
		/// The state to execute the statement with.
		/// </param>
		/// <returns>
		/// The next statement or null if there is no next statement.
		/// </returns>
		public RenPyStatement NextStatement(RenPyState state)
		{
			if(m_stack.Count > 0) {
				m_currentStatement = m_stack.Peek().NextStatement(state);
			}

			// If there are no more statements, dispose of that stack frame
			while (m_currentStatement == null) {
				if(m_stack.Count > 0) {
					m_stack.Pop();
				} else {
					break;
				}
				
				// Check if there are any more frames
				if(m_stack.Count > 0) {
					m_currentStatement = m_stack.Peek().NextStatement(state);
				} else {
					break;
				}
			}

			if (m_currentStatement != null) {
				Static.Log(m_currentStatement.ToString());

				var type = m_currentStatement.Type;
				if (type == RenPyStatementType.SAY && Static.SkipDialog) {
					state.Execution.NextStatement(state);
				}
			} else {
				Static.Log("Execution state reached end of script");
			}
			return m_currentStatement;
		}

		/// <summary>
		/// Resets the state of the RenPyDialogState.
		/// </summary>
		public void Reset()
		{
			m_initialStackFrame.Reset();
			m_stack.Clear();
			m_stack.Push(m_initialStackFrame);
			m_currentStatement = null;
		}
		
		/// <summary>
		/// Goes to the specified label in the Ren'Py script.
		/// </summary>
		/// <param name="label">
		/// The label to jump to.
		/// </param>
		public void GoToLabel(string label)
		{
			if (m_stack.Count == 0) {
				UnityEngine.Debug.LogError("There is no stack to search.");
				return;
			}

			while (!m_stack.Peek().HasLabel(label)) {
				m_stack.Pop();

				if (m_stack.Count == 0) {
					UnityEngine.Debug.LogError("Label " + label + " could not be "
						+ "found.");
					return;
				}
			}

			m_stack.Peek().GoToLabel(label);
		}
		
		/// <summary>
		/// Pushes a new stack frame onto the call stack with the passed list of
		/// RenPyBlocks.
		/// </summary>
		/// <param name="blocks">
		/// The list of RenPyBlocks to create the RenPyStackFrame with.
		/// </param>
		public void PushStackFrame(List<RenPyBlock> blocks)
		{
			m_stack.Push(new RenPyStackFrame(blocks));
		}

		/// <summary>
		/// Pops the top stack frame on the call stack.
		/// </summary>
		public void PopStackFrame()
		{
			m_stack.Pop();
		}

		/// <summary>
		/// Returns the previous statement.
		/// </summary>
		/// <returns>
		/// The previous statement or null if there is no previous statement.
		/// </returns>
		public RenPyStatement GetPreviousStatement()
		{
			if (m_stack.Count > 0) {
				return m_stack.Peek().GetPreviousStatement();
			}
			return null;
		}
	}
}
