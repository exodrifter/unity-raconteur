using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine display macro inserts a specified passage into the current
	/// passage.
	/// </summary>
	public class TwineDisplayMacro : TwineMacro
	{
		/// <summary>
		/// The expression, when evaluated, will return the name of the passage
		/// to insert.
		/// </summary>
		private Expression m_expression = null;
		
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
				string passage = tokens.Seek(">>");
				m_expression = parser.ParseExpression("\"" + passage + "\"");
			}

			tokens.Next();
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			string passage = m_expression.Evaluate(state).AsString(state);
			return state.Script.GetPassage(passage).Compile(state);
		}
		
		protected override string ToDebugString()
		{
			string str = "display ";
			str += m_expression.ToString();
			return str;
		}
	}
}
