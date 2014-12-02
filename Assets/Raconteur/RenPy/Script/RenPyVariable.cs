using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyVariable : RenPyStatement
	{
		[SerializeField]
		private Expression m_expression;
		public Expression Expression
		{
			get {
				return m_expression;
			}
		}

		public RenPyVariable() : base(RenPyStatementType.VARIABLE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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
