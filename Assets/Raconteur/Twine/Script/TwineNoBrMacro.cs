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
	public class TwineNoBrMacro : TwineMacro
	{
		List<TwineLine> lines;
		
		public TwineNoBrMacro(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("nobr");
			tokens.Next();
			tokens.Seek(">>");
			tokens.Next();
			
			// Find the endnobr macro
			string content = "";
			string macro = null;
			do
			{
				content += tokens.Seek("<<");
				macro = tokens.PeekIgnore(new string[] {"<<", " "});
				if(macro != "endnobr")
				{
					content += tokens.Next();
				}
			} while (macro != "endnobr" && tokens.HasNext());
			
			tokens.Seek("endnobr");
			tokens.Next();
			tokens.Seek(">>");
			tokens.Next();

			content = content.Replace("\n", "");
			lines = TwineParser.ParseLines(content);
		}
		
		public override List<TwineLine> Compile(TwineState state)
		{
			var list = new List<TwineLine>();
			
			foreach (var line in lines)
			{
				if (line is TwineMacro)
				{
					list.Add (line);
				}
			}
			return list;
		}
		
		protected override string ToDebugString()
		{
			return "nobr";
		}
	}
}
