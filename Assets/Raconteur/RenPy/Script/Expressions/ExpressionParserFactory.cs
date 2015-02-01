using UnityEngine;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A convenience class for getting Expression Parsers of a specific
	/// setup.
	/// </summary>
	public class ExpressionParserFactory
	{
		private static ExpressionParser m_renPyExpressionParser;

		private ExpressionParserFactory()
		{
			// Defeats instantiation
		}

		/// <summary>
		/// Returns a new ExpressionParser for Ren'Py expressions.
		/// </summary>
		/// <returns>
		/// An ExpressionParser for parsing Ren'Py expressions.ß
		/// </returns>
		public static ExpressionParser GetRenPyParser()
		{
			if(m_renPyExpressionParser != null) {
				return m_renPyExpressionParser;
			}
			var parser = m_renPyExpressionParser = new ExpressionParser();
			parser.SetupOperator(new OperatorAssignPlus("+="));
			parser.SetupOperator(new OperatorAssignMinus("-="));
			parser.SetupOperator(new OperatorAssignMultiply("*="));
			parser.SetupOperator(new OperatorAssignDivide("/="));
			parser.SetupOperator(new OperatorEquals("=="));
			parser.SetupOperator(new OperatorNotEquals("!="));
			parser.SetupOperator(new OperatorLessThanOrEqual("<="));
			parser.SetupOperator(new OperatorGreaterThanOrEqual(">="));
			parser.SetupOperator(new OperatorAssign("="));
			parser.SetupOperator(new OperatorPlus("+"));
			parser.SetupOperator(new OperatorMinus("-"));
			parser.SetupOperator(new OperatorMultiply("*"));
			parser.SetupOperator(new OperatorDivide("/"));
			parser.SetupOperator(new OperatorLessThan("<"));
			parser.SetupOperator(new OperatorGreaterThan(">"));

			return m_renPyExpressionParser;
		}
	}
}
