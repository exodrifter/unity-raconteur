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
		/// Moves the state to the next statement and returns that statement.
		/// </summary>
		/// <param name="state">
		/// The state to execute the statement with.
		/// </param>
		/// <returns>
		/// The next statement or null if there is no next statement.
		/// </returns>
		public RenPyStatement NextStatement(RenPyState state)
		{
			RenPyStatement statement = null;
			if(m_stack.Count > 0) {
				 statement = m_stack.Peek().NextStatement(state);
			}

			// If there are no more statements, dispose of that stack frame
			while(statement == null) {
				if(m_stack.Count > 0) {
					m_stack.Pop();
				} else {
					break;
				}
				
				// Check if there are any more frames
				if(m_stack.Count > 0) {
					statement = m_stack.Peek().NextStatement(state);
				} else {
					break;
				}
			}

			if(statement != null) {
				Static.Log(statement.ToString());
			} else {
				Static.Log("Execution state reached end of script");
			}
			return statement;
		}

		/// <summary>
		/// Resets the state of the RenPyDialogState.
		/// </summary>
		public void Reset()
		{
			m_initialStackFrame.Reset();

			m_stack.Clear();
			m_stack.Push(m_initialStackFrame);
		}
		
		/// <summary>
		/// Goes to the specified label in the Ren'Py script.
		/// </summary>
		/// <param name="label">
		/// The label to jump to.
		/// </param>
		public void GoToLabel(string label)
		{
			while(!m_stack.Peek().HasLabel(label)) {
				m_stack.Pop();
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
	}
}
