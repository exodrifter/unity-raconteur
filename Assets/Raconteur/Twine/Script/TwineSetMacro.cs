using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwineSetMacro : TwineLine
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

		public override string Print()
		{
			return null;
		}

		protected override string ToDebugString()
		{
			string ret = null;
			foreach (Expression expression in m_expressions)
			{
				if (ret == null)
				{
					ret = expression.ToString();
				}
				else
				{
					ret += "; " + expression.ToString();
				}
			}
			return ret;
		}
	}
}
