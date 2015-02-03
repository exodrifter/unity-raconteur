using UnityEngine;
using System;
using System.Collections.Generic;
using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// The Expression Parser parses simple expressions that involve arithmetic
	/// and assignment.
	/// </summary>
	public class ExpressionParser
	{
		/// <summary>
		/// A list of operators that this parser can parse.
		/// </summary>
		private List<Operator> m_operators;

		/// <summary>
		/// The tokenizer for this parser
		/// </summary>
		private Tokenizer m_tokenizer;

		/// <summary>
		/// Creates a new ExpressionParser. The parser will not be ready for use
		/// after this; the parser must be set up with additional calls to the
		/// methods that start with the word "Setup".
		/// </summary>
		public ExpressionParser()
		{
			m_operators = new List<Operator>();
			m_tokenizer = new Tokenizer(true);
		}

		#region Setup

		/// <summary>
		/// Adds an operator to parse.
		/// </summary>
		/// <param name="op">
		/// The operator to parse.
		/// </param>
		public void SetupOperator(Operator op)
		{
			if (op == null) {
				throw new ArgumentNullException("op");
			}

			m_tokenizer.SetupToken(op.Symbol);
			m_operators.Add(op);
		}

		#endregion

		#region Parsing

		public Expression ParseExpression(string str)
		{
			return ParseExpression(m_tokenizer.Tokenize(ref str));
		}
		
		// TODO: Parse parenthesis correctly
		private Expression ParseExpression(string[] tokens)
		{
			if(tokens.Length == 0) {
				throw new ArgumentException("Tokens may not be empty","tokens");
			}
			if(tokens.Length == 1) {
				return new Expression(new OperatorNoOp(""), tokens[0], null);
			}

			var right = new List<string>();
			for(int i = tokens.Length-1; i >= 0; --i)
			{
				string token = tokens[i];

				// Check if the token is an operator
				foreach(Operator op in m_operators)
				{
					if(op.Symbol == token)
					{
						string[] left = GetRemainder(i-1, tokens);

						Expression leftExp = ParseExpression(left);
						Expression rightExp = ParseExpression(right.ToArray());

						return new Expression(op, leftExp, rightExp);
					}
				}

				right.Add(token);
			}

			string list = "[";
			foreach(string token in tokens)
			{
				if(list.Length == 1)
				{
					list += token;
				}
				else
				{
					list += ", " + token;
				}
			}
			list += "]";
			string msg = "Tokens are not a valid expression string";
			msg += " (was:" + list+")";
			throw new ArgumentException(msg, "tokens");
		}

		private static string[] GetRemainder(int indexFrom, string[] arr)
		{
			var right = new List<string>();
			for(int i = indexFrom; i >= 0; --i)
			{
				right.Add(arr[i]);
			}
			
			return right.ToArray();
		}

		#endregion
	}
}
