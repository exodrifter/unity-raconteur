using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwineDisplayMacro : TwineLine
	{
		private Expression m_expression = null;
		private string m_passageName = null;
		
		public TwineDisplayMacro(ref Scanner tokens, bool shorthand)
		{
			var parser = ExpressionParserFactory.GetTwineParser();
			
			tokens.Seek("<<");
			tokens.Next();
			if (!shorthand)
			{
				tokens.Seek("display");
				tokens.Next();
				string expression = tokens.Seek(new string[] { ">>", ";" });
				m_expression = parser.ParseExpression(expression);
				tokens.Seek(">>");
			}
			else
			{
				m_passageName = tokens.Seek(">>");
			}

			tokens.Next();
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			string passageName = m_passageName;
			if (passageName == null)
			{
				passageName = m_expression.Evaluate(state).AsString(state);
			}

			return state.Script.GetPassage(passageName).Compile(state);
		}
		
		public override string Print()
		{
			return null;
		}
		
		protected override string ToDebugString()
		{
			return m_expression.ToString();
		}
	}
}
