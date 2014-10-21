using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Dialog;
using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy.Parser
{
	public class RenPyParser
	{
		public static RenPyDialogState Parse(ref RenPyScriptAsset script)
		{
			Tokenizer tokenizer = new Tokenizer();

			// Create the scanner
			LinkedList<string> tokens = new LinkedList<string>();
			for (int line = 0; line < script.Lines.Length; line++) {
				string[] arr = tokenizer.Tokenize(ref script.Lines[line]);
				for (int i = 0; i < arr.Length; i++) {
					tokens.AddLast(arr[i]);
				}
				tokens.AddLast("\n");
			}
			RenPyScanner scanner = new RenPyScanner(ref tokens);
			scanner.SkipWhitespace(true, true, true);

			// Parse the statements
			List<RenPyStatement> statements = new List<RenPyStatement>();
			while (scanner.HasNext()) {

				RenPyStatement statement = ParseStatement(ref scanner);
				if (statement != null) {
					statements.Add(statement);
				}

				scanner.SkipWhitespace(true, true, true);
			}

			return new RenPyDialogState(statements.ToArray());
		}

		private static RenPyStatement ParseStatement(ref RenPyScanner scanner)
		{
			switch (scanner.Peek()) {
				case "$":
					return new RenPyVariable(ref scanner);
				case "#":
					new RenPyComment(ref scanner);
					return null;
				case "if":
					return new RenPyIf(ref scanner);
				case "image":
					return new RenPyImage(ref scanner);
				case "jump":
					return new RenPyJump(ref scanner);
				case "define":
					return new RenPyCharacter(ref scanner);
				case "hide":
					return new RenPyHide(ref scanner);
				case "label":
					return new RenPyLabel(ref scanner);
				case "menu":
					return new RenPyMenu(ref scanner);
				case "pause":
					return new RenPyPause(ref scanner);
				case "play":
					return new RenPyPlay(ref scanner);
				case "return":
					return new RenPyReturn(ref scanner);
				case "scene":
					return new RenPyScene(ref scanner);
				case "show":
					return new RenPyShow(ref scanner);
				case "stop":
					return new RenPyStop(ref scanner);
				default:
					return new RenPySay(ref scanner);
			}
		}
	}
}
