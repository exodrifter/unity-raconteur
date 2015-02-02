using UnityEngine;

﻿using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;
using DPek.Raconteur.Util.Expressions;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py return statement.
	/// </summary>
	public class RenPyReturn : RenPyStatement
	{
		private string m_expressionString;
		private bool m_optionalExpressionUsed;

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
		public RenPyReturn(ref Scanner tokens) : base(RenPyStatementType.RETURN)
		{
			tokens.Seek("return");
			tokens.Next();

			// Check if there is anything after the return
			if(tokens.HasNext())
			{
				// Get the expression
				m_expressionString = tokens.Seek ("\0").Trim();
				tokens.Next();

				// Parse the expression
				var parser = ExpressionParserFactory.GetRenPyParser ();
				m_expression = parser.ParseExpression (m_expressionString);
				m_optionalExpressionUsed = true;
			}

		}

		//TODO Dynamically scope the _return variable for Milestone 3
		public override void Execute(RenPyState state)
		{
			//Evaluate the optional expression and store in _return
			if(m_optionalExpressionUsed)
				state.SetVariable ("_return", m_expression.Evaluate (state).ToString());
			state.Execution.PopStackFrame();
		}

		public override string ToDebugString()
		{
			string str = "return " + m_expressionString;
			return str;
		}
	}
}
