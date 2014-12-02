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
			parser.SetupOperator(Get<OperatorAssignPlus>("+="));
			parser.SetupOperator(Get<OperatorAssignMinus>("-="));
			parser.SetupOperator(Get<OperatorAssignMultiply>("*="));
			parser.SetupOperator(Get<OperatorAssignDivide>("/="));
			parser.SetupOperator(Get<OperatorEquals>("=="));
			parser.SetupOperator(Get<OperatorNotEquals>("!="));
			parser.SetupOperator(Get<OperatorLessThanOrEqual>("<="));
			parser.SetupOperator(Get<OperatorGreaterThanOrEqual>(">="));
			parser.SetupOperator(Get<OperatorAssign>("="));
			parser.SetupOperator(Get<OperatorPlus>("+"));
			parser.SetupOperator(Get<OperatorMinus>("-"));
			parser.SetupOperator(Get<OperatorLessThan>("<"));
			parser.SetupOperator(Get<OperatorGreaterThan>(">"));

			return m_renPyExpressionParser;
		}

		/// <summary>
		/// Returns a new Operator with the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol to use for the new Operator.
		/// </param>
		/// <typeparam name="T">
		/// The type of the Operator to get.
		/// </typeparam>
		private static Operator Get<T>(string symbol) where T : Operator
		{
			T op = ScriptableObject.CreateInstance<T>();
			op.Symbol = symbol;
			return op;
		}
	}
}
