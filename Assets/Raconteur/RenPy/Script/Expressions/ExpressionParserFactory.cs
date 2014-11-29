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
			parser.SetupOperator(Get<OperatorPlus>("+"));
			parser.SetupOperator(Get<OperatorMinus>("-"));
			parser.SetupOperator(Get<OperatorEquals>("=="));
			parser.SetupOperator(Get<OperatorNotEquals>("!="));
			parser.SetupOperator(Get<OperatorLessThan>("<"));
			parser.SetupOperator(Get<OperatorLessThanOrEqual>("<="));
			parser.SetupOperator(Get<OperatorGreaterThan>(">"));
			parser.SetupOperator(Get<OperatorGreaterThanOrEqual>(">="));

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
