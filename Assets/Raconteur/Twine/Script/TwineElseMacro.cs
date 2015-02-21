using DPek.Raconteur.Twine.Parser;
using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine silently macro hides anything that isn't a macro after it
	/// until the end silently macro is encountered.
	/// </summary>
	public class TwineElseMacro : TwineMacro
	{
		private Expression m_expression;
		private List<TwineLine> lines;
		
		/// <summary>
		/// The else macro, if any.
		/// </summary>
		private TwineElseMacro elseMacro;
		public TwineElseMacro ElseMacro
		{
			get { return elseMacro; }
			set { elseMacro = value; }
		}
		
		public TwineElseMacro(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("else");
			tokens.Next();
			if(tokens.PeekIgnore(new string[] {" "}) == "if")
			{
				tokens.Seek("if");
				tokens.Next();
				string expressionString = tokens.Seek(">>");
				var parser = ExpressionParserFactory.GetRenPyParser();
				m_expression = parser.ParseExpression(expressionString);
			}
			else
			{
				tokens.Seek(">>");
			}
			tokens.Next();
			
			// Find the endif or else macro
			string content = "";
			string macro = null;
			do
			{
				content += tokens.Seek("<<");
				macro = tokens.PeekIgnore(new string[] {"<<", " "});
				if(macro != "endif" && macro != "else")
				{
					content += tokens.Next();
				}
			} while (macro != "endif" && macro != "else" && tokens.HasNext());
			
			if(macro == "endif")
			{
				content += tokens.Seek("<<");
				tokens.Next();
				tokens.Seek("endif");
				tokens.Next();
				tokens.Seek(">>");
				tokens.Next();
			}
			else if (macro == "else")
			{
				elseMacro = new TwineElseMacro(ref tokens);
			}

			UnityEngine.Debug.LogWarning(content);
			lines = TwineParser.ParseLines(content);
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			// If evaluation succeeds, return the nested lines
			Value v = m_expression.Evaluate(state);
			if (v is ValueBoolean && (bool) v.GetRawValue(state))
			{
				Static.Log("else if " + m_expression + " evaluated to true");
				var list = new List<TwineLine>();
				foreach (var line in lines)
				{
					Static.Log (line.ToString());
					list.AddRange (line.Compile(state));
				}
				return list;
			}

			Static.Log("if " + m_expression + " evaluated to false");

			// If evaluation fails, go to the else statement
			if (elseMacro != null)
			{
				return elseMacro.Compile(state);
			}
			
			return new List<TwineLine>();
		}
		
		protected override string ToDebugString()
		{
			string str = "else ";
			str += m_expression != null ? "if " + m_expression.ToString() : " ";
			return str;
		}
	}
}
