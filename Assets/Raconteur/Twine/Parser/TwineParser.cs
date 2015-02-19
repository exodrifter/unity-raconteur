using DPek.Raconteur.Twine.Script;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;
using UnityEngine;

namespace DPek.Raconteur.Twine.Parser
{
	public class TwineParser
	{
		/// <summary>
		/// Attempts to parse the passed lines as a TwineStory. Throws a
		/// ParseException if parsing fails.
		/// </summary>
		/// <param name="content">
		/// The string to parse
		/// </param>
		/// <returns>
		/// The parsed TwineStory.
		/// </returns>
		public static TwineStory Parse(string content)
		{
			var story = new TwineStory();

			// Prepare the tokenizer
			var tokenizer = new Tokenizer(false);
			string[] parseTokens;
			parseTokens = "[[ ]] << >> // /% %/ | [ ] ( ) # \\\" \" ' , ; = + - * / \\ :: $".Split(' ');
			tokenizer.SetupTokens(parseTokens);

			// Create the scanner
			var tokens = new LinkedList<string>();
			string[] arr = tokenizer.Tokenize(ref content);
			for (int i = 0; i < arr.Length; i++)
			{
				tokens.AddLast(arr[i]);
			}
			var scanner = new Scanner(ref tokens);

			TwinePassage passage = null;
			while (scanner.HasNext())
			{
				string token = scanner.Peek();
				if (token == "::")
				{
					passage = new TwinePassage(ref scanner);
					if (passage.Title == "StoryTitle") {
						story.Title = scanner.Seek("::").TrimEnd();
					} else if (passage.Title == "StoryAuthor") {
						story.Author = scanner.Seek("::").TrimEnd();
					} else {
						story.AddPassage(passage);
					}
				}
				else if (token == "[[")
				{
					passage.Source.Add(new TwineLink(ref scanner));
				}
				else if (token == "<<")
				{
					string type = scanner.PeekIgnore(new string[] {"<<", " "});
					switch (type)
					{
						case "if":
							throw new ParseException("if macro not supported");
						case "else":
							throw new ParseException("else macro not supported");
						case "endif":
							throw new ParseException("endif macro not supported");
						case "set":
							passage.Source.Add(new TwineSetMacro(ref scanner));
						break;
						case "remember":
							passage.Source.Add(new TwineRememberMacro(ref scanner));
							break;
						case "forget":
							passage.Source.Add(new TwineForgetMacro(ref scanner));
							break;
						case "print":
							passage.Source.Add(new TwinePrintMacro(ref scanner, false));
							break;
						case "$":
							passage.Source.Add(new TwinePrintMacro(ref scanner, true));
							break;
						case "display":
							throw new ParseException("display macro not supported");
						case "actions":
							throw new ParseException("actions macro not supported");
						case "choice":
							throw new ParseException("choice macro not supported");
						case "nobr":
							throw new ParseException("nobr macro not supported");
						case "textinput":
							throw new ParseException("textinput macro not supported");
						case "radio":
							throw new ParseException("radio macro not supported");
						case "checkbox":
							throw new ParseException("checkbox macro not supported");
						case "button":
							throw new ParseException("button macro not supported");
						case "silently":
							throw new ParseException("silently macro not supported");
						case "back":
							throw new ParseException("back macro not supported");
						case "return":
							throw new ParseException("return macro not supported");
						default:
							throw new ParseException("unrecognized macro \"" + type + "\"");
					}
				}
				else
				{
					passage.Source.Add(new TwineEcho(ref scanner));
				}
			}

			return story;
		}
	}
}
