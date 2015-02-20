using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine set macro sets the value of any number of variables.
	/// </summary>
	public class TwineSetMacro : TwineMacro
	{
		private List<Expression> m_expressions;

		public TwineSetMacro(ref Scanner tokens)
		{
			m_expressions = new List<Expression>();
			var parser = ExpressionParserFactory.GetTwineParser();

			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("set");
			tokens.Next();

			do
			{
				tokens.Seek("$");
				tokens.Next();

				string expression = tokens.Seek(new string[] { ">>", ";" });
				m_expressions.Add(parser.ParseExpression(expression));
			} while (tokens.Peek() == ";");

			tokens.Seek(">>");
			tokens.Next();
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			foreach (Expression expression in m_expressions)
			{
				Static.Log (expression.ToString());
				expression.Evaluate(state);
			}
			return new List<TwineLine>();
		}

		protected override string ToDebugString()
		{
			string str = "set ";
			
			bool first = true;
			foreach (Expression expression in m_expressions)
			{
				if (!first)
				{
					str += "; ";
				}
				str += expression.ToString();
				first = false;
			}
			return str;
		}
	}
}
