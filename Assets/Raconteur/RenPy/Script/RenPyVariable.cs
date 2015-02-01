using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyVariable : RenPyStatement
	{
		private Expression m_expression;
		public Expression Expression
		{
			get {
				return m_expression;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyVariable(ref Scanner tokens) : base(RenPyStatementType.VARIABLE)
		{
			tokens.Seek("$");
			tokens.Next();

			// Get the expression
			string expressionString = tokens.Seek("\n").Trim();
			tokens.Next();

			var parser = ExpressionParserFactory.GetRenPyParser();
			m_expression = parser.ParseExpression(expressionString);
		}

		public override void Execute(RenPyState state)
		{
			m_expression.Evaluate(state);
		}

		public override string ToDebugString()
		{
			return "$ " + m_expression;
		}
	}
}
