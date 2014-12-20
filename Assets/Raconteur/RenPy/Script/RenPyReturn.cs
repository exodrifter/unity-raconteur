using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py return statement.
	/// </summary>
	public class RenPyReturn : RenPyStatement
	{
		[SerializeField]
		private Expression m_expression;
		private Value m_return;
		public Expression Expression
		{
			get {
				return m_expression;
			}
		}

		public RenPyReturn() : base(RenPyStatementType.RETURN)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("return");
			tokens.Next();

			// If there is anything left after the return
			if(tokens.Peek().Length > 0)
			{
				//string expressionString = tokens.Seek ("\n").Trim();
				//tokens.Next();

				// Parse the expression
				//var parser = ExpressionParserFactory.GetRenPyParser ();
				//m_expression = parser.ParseExpression (expressionString);
			}

		}

		//TODO Dynamically scope the _return variable for milestone 3
		public override void Execute(RenPyState state)
		{
			//Evaluate the optional expression and store in _return
			//state.SetVariable ("_return", m_expression.Evaluate (state).ToString());
			state.Execution.PopStackFrame();
		}

		public override string ToDebugString()
		{
			string str = "return +" + m_expression;
			return str;
		}
	}
}
