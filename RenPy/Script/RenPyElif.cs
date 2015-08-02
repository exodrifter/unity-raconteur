using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;
using DPek.Raconteur.Util.Expressions;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py elif statement.
	/// </summary>
	public class RenPyElif : RenPyStatement
	{
		private Expression m_expression;
		public Expression Expression
		{
			get
			{
				return m_expression;
			}
		}

		/// <summary>
		/// Whether or not last time this statement was executed that it
		/// or some if statement before it evaluated as true.
		/// </summary>
		private bool m_wasSuccessful;
		public bool WasSuccessful
		{
			get
			{
				return m_wasSuccessful;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyElif(ref Scanner tokens) : base(RenPyStatementType.ELIF)
		{
			tokens.Seek("elif");
			tokens.Next();

			// Get the expression
			string expressionString = tokens.Seek(":").Trim();
			tokens.Next();

			var parser = ExpressionParserFactory.GetRenPyParser();
			m_expression = parser.ParseExpression(expressionString);

			m_wasSuccessful = false;
		}

		public override void Execute(RenPyState state)
		{
			// Check if evaluation is necessary
			var prev = state.Execution.GetPreviousStatement() as RenPyIf;
			if (prev == null) {
				var elif = state.Execution.GetPreviousStatement() as RenPyElif;
				if (elif == null) {
					var msg = "elif expression has no preceding if statement";
					UnityEngine.Debug.LogError(msg);
					return;
				}
				else if (elif.WasSuccessful) {
					m_wasSuccessful = true;
					return;
				}
			}
			else if (prev.WasSuccessful) {
				m_wasSuccessful = true;
				return;
			}

			// If evaluation succeeds, push back this block
			Value v = m_expression.Evaluate(state);
			if (v is ValueBoolean && (bool) v.GetRawValue(state)) {
				string msg = "elif " + m_expression + " evaluated to true";
				Static.Log(msg);

				m_wasSuccessful = true;
				state.Execution.PushStackFrame(NestedBlocks);
			}
			// If evaluation fails, skip this block
			else {
				string msg = "elif " + m_expression + " evaluated to false";
				Static.Log(msg);

				m_wasSuccessful = false;
			}
		}

		public override string ToDebugString()
		{
			string str = "elif " + m_expression + ":";
			return str;
		}
	}
}
