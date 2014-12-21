using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py if statement.
	/// </summary>
	public class RenPyWhile : RenPyStatement
	{
		[SerializeField]
		private Expression m_expression;
		public Expression Expression
		{
			get {
				return m_expression;
			}
		}
		
		public RenPyWhile() : base(RenPyStatementType.WHILE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
		{
			tokens.Seek("while");
			tokens.Next();
			
			// Get the expression
			string expressionString = tokens.Seek(":").Trim();
			tokens.Next();
			
			var parser = ExpressionParserFactory.GetRenPyParser();
			m_expression = parser.ParseExpression(expressionString);
		}
		
		public override void Execute(RenPyState state)
		{
			// If evaluation succeeds, push back this block
			if (m_expression.Evaluate(state).GetValue(state).AsString(state) == "True") {
				string msg = "while " + m_expression + " evaluated to true";
				Static.Log(msg);

				state.Execution.PreviousStatement();
				state.Execution.PushStackFrame(NestedBlocks);
			}
			// If evaluation fails, skip this block
			else {
				string msg = "while " + m_expression + " evaluated to false";
				Static.Log(msg);
			}
		}
		
		public override string ToDebugString()
		{
			string str = "while " + m_expression + ":";
			return str;
		}
	}
}
