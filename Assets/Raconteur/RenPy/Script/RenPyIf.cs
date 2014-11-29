using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py if statement.
	/// </summary>
	public class RenPyIf : RenPyStatement
	{
		[SerializeField]
		private Expression m_expression;
		public Expression Expression
		{
			get {
				return m_expression;
			}
		}

		public RenPyIf() : base(RenPyStatementType.IF)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("if");
			tokens.Next();

			// Get the expression
			string expressionString = tokens.Seek(":").Trim();
			tokens.Next();
			
			var parser = new ExpressionParser();
			parser.SetupOperator(Get<OperatorPlus>("+"));
			parser.SetupOperator(Get<OperatorMinus>("-"));
			parser.SetupOperator(Get<OperatorEquals>("=="));
			parser.SetupOperator(Get<OperatorNotEquals>("!="));
			parser.SetupOperator(Get<OperatorLessThan>("<"));
			parser.SetupOperator(Get<OperatorLessThanOrEqual>("<="));
			parser.SetupOperator(Get<OperatorGreaterThan>(">"));
			parser.SetupOperator(Get<OperatorGreaterThanOrEqual>(">="));

			m_expression = parser.ParseExpression(expressionString);
		}

		Operator Get<T>(string symbol) where T : Operator {
			T op = ScriptableObject.CreateInstance<T>();
			op.Symbol = symbol;
			return op;
		}

		public override void Execute(RenPyState state)
		{
			// If evaluation succeeds, push back this block
			if (m_expression.Evaluate(state).GetValue(state).AsString(state) == "True") {
				string msg = "if " + m_expression + " evaluated to true";
				Static.Log(msg);

				state.Execution.PushStackFrame(NestedBlocks);
			}
			// If evaluation fails, skip this block
			else {
				string msg = "if " + m_expression + " evaluated to false";
				Static.Log(msg);
			}
		}

		public override string ToDebugString()
		{
			string str = "if " + m_expression + ":";
			return str;
		}
	}
}
