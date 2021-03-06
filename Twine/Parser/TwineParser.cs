﻿using DPek.Raconteur.Twine.Script;
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
			var tokens = TokenizeString(content);
			var scanner = new Scanner(ref tokens);

			TwinePassage passage = null;
			while (scanner.IsValid())
			{
				string token = scanner.Peek();
				if (token == "::")
				{
					passage = new TwinePassage(ref scanner);
					if (passage.Title == "StoryTitle") {
						story.Title = scanner.Seek("::").Trim();
					} else if (passage.Title == "StoryAuthor") {
						story.Author = scanner.Seek("::").Trim();
					} else {
						story.AddPassage(passage);
					}
				}
				else
				{
					var str = scanner.Seek("::");
					var lines = ParseLines(str);
					passage.Source.AddRange(lines);
				}
			}

			return story;
		}


		public static List<TwineLine> ParseLines(string content)
		{
			var lines = new List<TwineLine>();
			var tokens = TokenizeString(content);
			var scanner = new Scanner(ref tokens);

			while (scanner.IsValid())
			{
				string token = scanner.Peek();
				if (token == "[[")
				{
					lines.Add(new TwineLink(ref scanner));
				}
				else if (token == "<<")
				{
					string type = scanner.PeekIgnore(new string[] {"<<", " "});
					switch (type)
					{
					case "if":
						lines.Add(new TwineIfMacro(ref scanner));
						break;
					case "else":
						throw new ParseException("unexpected else macro");
					case "endif":
						throw new ParseException("unexpected endif macro");
					case "set":
						lines.Add(new TwineSetMacro(ref scanner));
						break;
					case "remember":
						lines.Add(new TwineRememberMacro(ref scanner));
						break;
					case "forget":
						lines.Add(new TwineForgetMacro(ref scanner));
						break;
					case "print":
						lines.Add(new TwinePrintMacro(ref scanner, false));
						break;
					case "$":
						lines.Add(new TwinePrintMacro(ref scanner, true));
						break;
					case "display":
						lines.Add(new TwineDisplayMacro(ref scanner, false));
						break;
					case "actions":
						lines.Add(new TwineActionsMacro(ref scanner));
						break;
					case "choice":
						lines.Add(new TwineChoiceMacro(ref scanner));
						break;
					case "nobr":
						lines.Add(new TwineNoBrMacro(ref scanner));
						break;
					case "textinput":
						throw new ParseException("textinput macro not supported");
					case "radio":
						throw new ParseException("radio macro not supported");
					case "checkbox":
						throw new ParseException("checkbox macro not supported");
					case "button":
						throw new ParseException("button macro not supported");
					case "silently":
						lines.Add(new TwineSilentlyMacro(ref scanner));
						break;
					case "back":
						throw new ParseException("back macro not supported");
					case "return":
						throw new ParseException("return macro not supported");
					default:
						lines.Add(new TwineDisplayMacro(ref scanner, true));
						break;
					}
				}
				else
				{
					lines.Add(new TwineEcho(ref scanner));
				}
			}
			return lines;
		}

		private static LinkedList<string> TokenizeString(string content)
		{
			// Prepare the tokenizer
			var tokenizer = new Tokenizer();
			string[] parseTokens;
			parseTokens = "[[ ]] << >> // /% %/ | [ ] ( ) # \\\" \" ' , ; = + - * / \\ :: $".Split(' ');
			tokenizer.SetupTokens(parseTokens);
			tokenizer.SetupTokens(new string[] { " ", "\t", "\n" });

			// Create the scanner
			var tokens = new LinkedList<string>();
			string[] arr = tokenizer.Tokenize(ref content);
			for (int i = 0; i < arr.Length; i++)
			{
				tokens.AddLast(arr[i]);
			}

			return tokens;
		}
	}
}
