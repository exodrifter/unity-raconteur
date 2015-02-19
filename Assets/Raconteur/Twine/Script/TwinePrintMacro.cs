using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwinePrintMacro : TwineLine
	{
		private Expression m_expression;
		
		public TwinePrintMacro(ref Scanner tokens, bool shorthand)
		{
			var parser = ExpressionParserFactory.GetTwineParser();
			
			tokens.Seek("<<");
			tokens.Next();
			if (!shorthand)
			{
				tokens.Seek("print");
				tokens.Next();
			}

			tokens.Seek("$");
			tokens.Next();
			
			string expression = tokens.Seek(new string[] { ">>", ";" });
			m_expression = parser.ParseExpression(expression);
			
			tokens.Seek(">>");
			tokens.Next();
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			Static.Log(m_expression.ToString());
			Value val = m_expression.Evaluate(state);

			var list = new List<TwineLine>();
			list.Add (new TwineEcho(val.AsString(state)));
			return list;
		}
		
		public override string Print()
		{
			return null;
		}
		
		protected override string ToDebugString()
		{
			string ret = m_expression.ToString();
			return ret;
		}
	}
}
