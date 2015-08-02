using UnityEngine;

namespace DPek.Raconteur.Util.Expressions
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
		/// An ExpressionParser for parsing Ren'Py expressions.
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
			parser.SetupOperator(new OperatorAnd(" and "));
			parser.SetupOperator(new OperatorOr(" or "));

			return m_renPyExpressionParser;
		}

		/// <summary>
		/// Returns a new ExpressionParser for Twine expressions.
		/// </summary>
		/// <returns>
		/// An ExpressionParser for parsing Twine expressions.
		/// </returns>
		public static ExpressionParser GetTwineParser()
		{
			if (m_renPyExpressionParser != null)
			{
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
			parser.SetupOperator(new OperatorEquals(" is "));
			parser.SetupOperator(new OperatorNotEquals(" neq "));
			parser.SetupOperator(new OperatorAssign(" to "));
			parser.SetupOperator(new OperatorAnd(" and "));
			parser.SetupOperator(new OperatorOr(" or "));

			return m_renPyExpressionParser;
		}
	}
}
