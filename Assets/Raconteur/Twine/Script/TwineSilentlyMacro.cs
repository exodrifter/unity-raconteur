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
	public class TwineSilentlyMacro : TwineMacro
	{
		List<TwineLine> lines;

		public TwineSilentlyMacro(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("silently");
			tokens.Next();
			tokens.Seek(">>");
			tokens.Next();

			// Find the endsilently macro
			string content = "";
			string macro = null;
			do
			{
				content += tokens.Seek("<<");
				macro = tokens.PeekIgnore(new string[] {"<<", " "});
				if(macro != "endsilently")
				{
					content += tokens.Next();
				}
			} while (macro != "endsilently" && tokens.HasNext());

			tokens.Seek("endsilently");
			tokens.Next();
			tokens.Seek(">>");
			tokens.Next();

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
			return "silently";
		}
	}
}
