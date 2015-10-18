using UnityEngine;
using System.Collections.Generic;

namespace Exodrifter.Raconteur.RenPy
{
	public static class RenPyParser
	{
		public static void Parse(RenPyScriptAsset script)
		{
			var lines = ParseLogicalLines(script);

			int i = 0;
			var blocks = ParseBlocks(lines, ref i);
		}

		private static LogicalLine[] ParseLogicalLines(RenPyScriptAsset script)
		{
			string name = script.name;
			string source = script.Source;

			int pos = 0;
			int parenDepth = 0;
			int lineNumber = 1;
			var lines = new List<LogicalLine>();

			// Skip BOM
			if (source.Length > 0 && '\ufeff' == source[0])
				pos += 1;

			while (pos < source.Length) {

				int startNumber = lineNumber;
				string str = "";

				while (pos < source.Length) {
					var ch = source[pos];
					++pos;

					// Tabs
					if ('\t' == ch) {
						string msg = "Tab characters are not allowed in Ren'Py scripts.";
						throw new RenPyParseException(name, lineNumber, msg);
					}

					// Newlines
					if ('\n' == ch && parenDepth == 0) {
						++lineNumber;
						break;
					}

					// Backslash/newline
					if ('\\' == ch && '\n' == source[pos + 1]) {
						++pos;
						++lineNumber;
						str += "\\\n";
						continue;
					}

					// Parenthesis
					if ('(' == ch || '[' == ch || '{' == ch)
						++parenDepth;
					if ((')' == ch || ']' == ch || '}' == ch) && parenDepth > 0)
						--parenDepth;

					// Comments
					if ('#' == ch) {
						while ('\n' != source[pos])
							pos += 1;
						continue;
					}

					// Strings
					if ('\'' == ch || '\"' == ch || '`' == ch) {
						var delim = ch;
						str += ch;
						bool escape = true;

						while (pos < source.Length) {
							ch = source[pos];
							++pos;

							if ('\n' == ch) {
								++lineNumber;
							}

							if (escape) {
								escape = false;
								str += ch;
								continue;
							}

							if (delim == ch) {
								str += ch;
								break;
							}

							if ('\\' == ch) {
								escape = true;
							}

							str += ch;
						}

						continue;
					}

					str += ch;
				}

				if ("" != str.Trim()) {
					var text = str.Trim(new char[] { ' ' });
					int level = str.TrimEnd(new char[] { ' ' }).Length - text.Length;

					lines.Add(new LogicalLine(startNumber, level, text));
				}
			}

			return lines.ToArray();
		}

		private static LogicalBlock[] ParseBlocks(LogicalLine[] lines, ref int index)
		{
			var blocks = new List<LogicalBlock>();
			int currentDepth = lines[index].depth;

			while (index < lines.Length) {
				var line = lines[index];

				// Part of block
				if (line.depth == currentDepth) {
					blocks.Add(new LogicalBlock(line, null));
				}

				// New nested block
				else if (line.depth > currentDepth) {
					var nested = ParseBlocks(lines, ref index);
					int i = blocks.Count - 1;
					blocks[i] = new LogicalBlock(blocks[i].line, nested);
					continue;
				}

				// End of block
				else if (line.depth < currentDepth) {
					break;
				}

				index++;
			}

			return blocks.ToArray();
		}
	}

	struct LogicalLine
	{
		public readonly int lineNumber;
		public readonly int depth;
		public readonly string str;

		public LogicalLine(int lineNumber, int level, string str)
		{
			this.lineNumber = lineNumber;
			this.depth = level;
			this.str = str;
		}
	}

	struct LogicalBlock
	{
		public readonly LogicalLine line;
		public readonly LogicalBlock[] nested;

		public LogicalBlock(LogicalLine line, LogicalBlock[] nested)
		{
			this.line = line;
			this.nested = nested ?? new LogicalBlock[0];
		}
	}
}
