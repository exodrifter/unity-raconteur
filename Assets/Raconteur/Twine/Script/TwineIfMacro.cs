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
	public class TwineIfMacro : TwineMacro
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
		
		public TwineIfMacro(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("if");
			tokens.Next();
			string expressionString = tokens.Seek(">>");
			tokens.Next();

			var parser = ExpressionParserFactory.GetRenPyParser();
			m_expression = parser.ParseExpression(expressionString);
			
			// Find the endif or else macro
			string content = "";
			string macro = null;
			int nestedIfs = 0;
			do
			{
				content += tokens.Seek("<<");
				macro = tokens.PeekIgnore(new string[] {"<<", " "});

				if(macro == "if")
				{
					content += tokens.Next();
					nestedIfs++;
				}
				else if(macro == "endif")
				{
					if(nestedIfs > 0)
					{
						content += tokens.Seek(">>");
						content += tokens.Next();
					}
					nestedIfs--;
				}
				else if(macro != "endif" && macro != "else")
				{
					content += tokens.Next();
				}
			} while (tokens.HasNext()
			    && (nestedIfs >= 0 || (macro != "endif" && macro != "else")));

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

			lines = TwineParser.ParseLines(content);
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			// If evaluation succeeds, return the nested lines
			Value v = m_expression.Evaluate(state);
			if (v is ValueBoolean && (bool) v.GetRawValue(state))
			{
				Static.Log("if " + m_expression + " evaluated to true");

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
			return "if " + m_expression;
		}
	}
}
