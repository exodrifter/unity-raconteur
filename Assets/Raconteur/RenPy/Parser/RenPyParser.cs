using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DPek.Raconteur.RenPy.Script;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Parser
{
	public class RenPyParser
	{
		public static RenPyState Parse(ref RenPyScriptAsset script)
		{
			var tokenizer = new Tokenizer();

			// Create the scanner
			var tokens = new LinkedList<string>();
			for (int line = 0; line < script.Lines.Length; line++) {
				string[] arr = tokenizer.Tokenize(ref script.Lines[line]);
				for (int i = 0; i < arr.Length; i++) {
					tokens.AddLast(arr[i]);
				}
				tokens.AddLast("\n");
			}
			var scanner = new RenPyScanner(ref tokens);

			// Parse the statements
			var statements = new List<RenPyStatement>();
			var levels = new List<int>();
			scanner.SkipEmptyLines();
			while (scanner.HasNext()) {

				int level = scanner.SkipWhitespace();

				RenPyStatement statement = ParseStatement(ref scanner);
				if (statement != null) {
					statements.Add(statement);
					levels.Add(level);
				}

				scanner.SkipEmptyLines();
			}

			// Create blocks out of the statements
			int startIndex = 0;
			var blocks = ParseBlocks(ref statements, ref levels, ref startIndex);

			// Get init statements in order from low to high priority
			RenPyInitPhase initPhase = ExtractInitPhase(blocks);
			var initBlocks = new List<RenPyBlock>();
			foreach(var kvp in initPhase.Statements.OrderBy(i => i.Key)) {
				var block = new RenPyBlock(kvp.Value);
				initBlocks.Add(block);
			}

			// Combine init blocks with the rest of the blocks
			initBlocks.AddRange(blocks);

			return new RenPyState(ref script, ref initBlocks);
		}

		/// <summary>
		/// Extracts and removes the init phase from the passed list of
		/// RenPyBlocks.
		/// </summary>
		/// <returns>
		/// The init phase for the passed blocks.
		/// </returns>
		/// <param name="blocks">
		/// A list of RenPyBlocks to extract the init phase from.
		/// </param>
		private static RenPyInitPhase ExtractInitPhase(List<RenPyBlock> blocks) {
			RenPyInitPhase initPhase = new RenPyInitPhase();

			// Read every statement in this block
			for (int i = 0 ; i < blocks.Count; ++i) {
				var block = blocks[i];
				for(int j = 0; j < block.StatementCount; ++j) {
					var statement = block[j];
					bool remove = false;

					// Look for statements that must be part of the init phase
					if (statement is RenPyInit) {
						int priority = (statement as RenPyInit).Priority;
						initPhase.AddStatement(priority, statement);
						remove = true;
					} else if (statement is RenPyCharacter) {
						initPhase.AddStatement(0, statement);
						remove = true;
					} else if (statement is RenPyImage) {
						initPhase.AddStatement(990, statement);
						remove = true;
					}
					// Look for init phase statements inside nested blocks
					else if (statement.NestedBlocks != null) {
						var addPhase = ExtractInitPhase(statement.NestedBlocks);
						initPhase.AddPhase(ref addPhase);
					}
					
					// Remove the statement if needed
					if(remove) {
						block.RemoveAt(j);
						--j;
					}
				}
			}
			return initPhase;
		}

		private static List<RenPyBlock> ParseBlocks(ref List<RenPyStatement> statements,
		                                            ref List<int> levels,
		                                            ref int index)
		{
			var blocks = new List<RenPyBlock>();
			int currentLevel = levels[index];
			var statementBlock = new List<RenPyStatement>();

			for (; index < statements.Count; ++index)
			{
				// Belongs to this block
				if (levels[index] == currentLevel) {
					statementBlock.Add(statements[index]);
				}

				// Reached the end of this block
				else if (levels[index] < currentLevel) {
					if (statementBlock.Count > 0) {
						blocks.Add(new RenPyBlock(statementBlock));
					}
					--index;
					return blocks;
				}

				// Belongs to a nested block
				else {
					var previousStatement = statementBlock[statementBlock.Count - 1];
					var nestedBlocks = ParseBlocks(ref statements, ref levels, ref index);
					previousStatement.NestedBlocks = nestedBlocks;
				}
			}

			if (statementBlock.Count > 0) {
				blocks.Add(new RenPyBlock(statementBlock));
			}
			return blocks;
		}

		private static RenPyStatement ParseStatement(ref RenPyScanner scanner)
		{
			switch (scanner.Peek()) {
				case "$":
					return new RenPyVariable(ref scanner);
				case "#":
					new RenPyComment(ref scanner);
					return null;
				case "call":
					return new RenPyCall(ref scanner);
				case "define":
					return new RenPyCharacter(ref scanner);
				case "hide":
					return new RenPyHide(ref scanner);
				case "if":
					return new RenPyIf(ref scanner);
				case "init":
					return new RenPyInit(ref scanner);
				case "image":
					return new RenPyImage(ref scanner);
				case "jump":
					return new RenPyJump(ref scanner);
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
