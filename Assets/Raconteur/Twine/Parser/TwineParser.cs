using DPek.Raconteur.Twine.Script;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Parser
{
	public class TwineParser
	{
		public static TwineStory Parse(string[] lines)
		{
			var story = new TwineStory();

			var tokenizer = new Tokenizer(false);
			string[] parseTokens;
			parseTokens = "[[ ]] << >> | [ ] ( ) # \\\" \" ' , ; = + - * / \\ :: $".Split(' ');
			tokenizer.SetupTokens(parseTokens);

			// Create the scanner
			var tokens = new LinkedList<string>();
			for (int line = 0; line < lines.Length; line++)
			{
				string[] arr = tokenizer.Tokenize(ref lines[line]);
				for (int i = 0; i < arr.Length; i++)
				{
					tokens.AddLast(arr[i]);
				}
				tokens.AddLast("\n");
			}
			var scanner = new Scanner(ref tokens);

			TwinePassage passage = null;
			scanner.SkipEmptyLines();
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
					passage.Lines.Add(new TwineLink(ref scanner));
				}
				else if (token == "<<")
				{
					scanner.Next(); // TODO: Parse
				}
				else
				{
					passage.Lines.Add(new TwineEcho(ref scanner));
				}
			}

			return story;
		}
	}
}
