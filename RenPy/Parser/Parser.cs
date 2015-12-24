using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Exodrifter.Raconteur.RenPy.Util;

using py = Exodrifter.Raconteur.RenPy.Util.Python;
using re = Exodrifter.Raconteur.RenPy.Util.PythonRegex;
using Triple = Exodrifter.Raconteur.RenPy.Util.Tuple<string, int, string>;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// A C# translation of functions in the renpy/parser.py file. This file
	/// does not contain the entire of parser.py translated in C#; instead, it
	/// is a class that represents some of the many functions found in that
	/// file.
	/// 
	/// This module contains the parser for the Ren'Py script language. It's
	/// called when parsing is necessary, and creates an AST from the script.
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// A list of parse error messages.
		/// </summary>
		public static List<ParseError> parse_errors = new List<ParseError> ();

		/// <summary>
		/// Matches either a word, or something else. Most magic is taken care
		/// of before this.
		/// </summary>
		public static Regex lllword = re.compile ("__(\\w+)|\\w+| +|.", re.S);

		/// <summary>
		/// The filename that the start and end positions are relative to.
		/// </summary>
		public static string original_filename;

		private static Dictionary<int, LogicalLine> lines;

		private static ParseTrie statements = new ParseTrie ();

		#region Mangling

		private static string munge_filename (string fn)
		{
			// The prefix that's used when __ is found in the file.
			var rv = Path.GetFileNameWithoutExtension (fn);
			rv = rv.Replace (' ', '_');

			Func<Match, string> munge_char =
				(Match m) => { return py.hex (py.ord (m.Value)); };

			rv = re.sub ("[^a-zA-Z0-9_]", munge_char, rv);

			return "_m1_" + rv + "__";
		}

		/// <summary>
		/// This function is a placeholder; the absolute path of the file
		/// should already be known, so eliding is not needed.
		/// </summary>
		private static string elide_filename (string fn)
		{
			return fn;
		}

		/// <summary>
		/// This function is a placeholder; the absolute path of the file
		/// should already be known, so uneliding is not needed.
		/// </summary>
		private static string unelide_filename (string fn)
		{
			return fn;
		}

		#endregion

		/// <summary>
		/// Reads `filename`, and divides it into logical lines.
		/// 
		/// Returns a list of (filename, line number, line text) triples.
		/// 
		/// If `filedata` is given, it should be a unicode string giving the file
		/// contents. In that case, `filename` need not exist.
		/// </summary>
		/// <returns>
		/// A list of (filename, line number, line text) triples.
		/// </returns>
		/// <param name="filename">
		/// The absolute path to the file.
		/// </param>
		/// <param name="filedata">
		/// A unicode string giving the file contents.
		/// </param>
		/// <param name="linenumber">
		/// If given, the parse starts at that line number.
		/// </param>
		private List<Triple> list_logical_lines (string filename, string filedata = null, int linenumber = 1)
		{
			original_filename = filename;

			string data = filedata;
			if (filedata == null) {
				var path = filename;
				data = File.ReadAllText (path, Encoding.UTF8);
			}

			filename = elide_filename (filename);
			var prefix = munge_filename (filename);

			// Add some newlines, to fix lousy editors.
			data += "\n\n";

			// The result
			var rv = new List<Triple> ();

			// The line number in the physical file.
			var number = linenumber;

			// The current position we're looking at in the buffer.
			var pos = 0;

			// Skip the BOM, if any.
			if (data.Length > 0 && data[0] == '\ufeff') {
				pos += 1;
			}

			lines = new Dictionary<int, LogicalLine> ();
			// TODO: What is init_phase?
//			if renpy.game.context().init_phase:
//				lines = renpy.scriptedit.lines

			string line = "";
			int start_number = -1;

			while (pos < data.Length)
			{
				// The line number of the start of this logical line.
				start_number = number;

				// The line that we're building up.
				line = "";

				// The number of open parenthesis there are right now.
				var parendepth = 0;

				var loc = start_number;
				lines.Add (loc, new LogicalLine (filename, start_number, pos));

				int? endpos = null;

				while (pos < data.Length)
				{
					char c = data[pos];

					if (c == '\t') {
						throw new ParseError (filename, number, "Tab characters "
							+ "are not allowed in Ren'Py scripts.");
					}

					if (c == '\n' && parendepth == 0)
					{
						// If not blank...
						if (!re.match ("^\\s*$", line))
						{
							// Add to the results.
							rv.Add (new Triple (filename, start_number, line));

							if (endpos == null) {
								endpos = pos;
							}

							lines[loc].end_delim = endpos.Value + 1;

							while (data[endpos.Value-1].In (" \r")) {
								endpos -= 1;
							}

							lines[loc].end = endpos.Value;
							lines[loc].text = data.Slice (lines[loc].start, lines[loc].end);
						}

						pos += 1;
						number += 1;
						endpos = null;
						// This helps out error checking.
						line = "";
						break;
					}

					if (c == '\n')
					{
						number += 1;
						endpos = null;
					}

					if (c == '\r')
					{
						pos += 1;
						continue;
					}

					// Backslash/newline.
					if (c == '\\' && data[pos+1] == '\n')
					{
						pos += 2;
						number += 1;
						line += "\\\n";
						continue;
					}

					// Parenthesis.
					if (c.In ('(', '[', '{'))
					{
						parendepth += 1;
					}
					if (c.In ('}', ']', ')') && parendepth > 0)
					{
						parendepth -= 1;
					}

					// Comments.
					if (c == '#')
					{
						endpos = pos;

						while (data[pos] != '\n') {
							pos += 1;
						}

						continue;
					}

					// Strings.
					if (c.In ('\"', '\'', '`'))
					{
						var delim = c;
						line += c;
						pos += 1;

						var escape = false;

						while (pos < data.Length)
						{
							c = data[pos];

							if (c == '\n') {
								number += 1;
							}

							if (c == '\r') {
								pos += 1;
								continue;
							}

							if (escape) {
								escape = false;
								pos += 1;
								line += c;
								continue;
							}

							if (c == delim) {
								pos += 1;
								line += c;
								break;
							}

							if (c == '\\') {
								escape = false;
							}

							line += c;
							pos += 1;

							continue;
						}

						continue;
					}

					// TODO: RenPy uses the re.S constant, which forces the '.'
					// to also match newlines.
					var m = lllword.Matches (data, pos);

					var word = m[0].Value;
					var rest = m[1].Value;

					if (rest != null && !"__".In (rest)) {
						word = prefix + rest;
					}

					line += word;

					if (line.Length > 65536)
					{
						// TODO: The original source has more arguments
						throw new ParseError (filename, start_number, "Overly long logical line. (Check strings and parenthesis.)");
					}

					// TODO: What is end(int)?
					//pos = m.end(0);
				}
			}

			if (line != "") {
				// TODO: The original source has more arguments
				throw new ParseError (filename, start_number, "is not terminated with a newline. (Check strings and parenthesis.)");
			}

			return rv;
		}

		/// <summary>
		/// Returns the depth of a line, and the rest of the line.
		/// </summary>
		/// <param name="lines">The line to get the depth of.</param>
		private Tuple<int, string> depth_split (string l)
		{
			var depth = 0;
			var index = 0;

			while (true)
			{
				if (l[index] == ' ')
				{
					depth += 1;
					index += 1;
					continue;
				}

//				if (l[index] == '\t')
//				{
//					index += 1;
//					depth = depth + 8 - (depth % 8);
//					continue;
//				}

				break;
			}

			return new Tuple<int, string> (depth, l.Slice (index, null));
		}

		private Tuple<List<Block>, int> gll_core (int i, int min_depth)
		{
			List<Block> rv = new List<Block> ();
			int? depth = null;

			int[] keys = new int[lines.Keys.Count];
			lines.Keys.CopyTo (keys, 0);

			while (i < keys.Length)
			{
				string filename = lines[keys[i]].filename;
				int number = lines[keys[i]].number;
				string text = lines[keys[i]].text;

				var ds = depth_split (text);
				var line_depth = ds.a;
				var rest = ds.b;

				// This catches a block exit
				if (line_depth < min_depth) {
					break;
				}

				if (depth == null) {
					depth = line_depth;
				}

				if (depth != line_depth) {
					throw new ParseError (filename, number, "indentation mismatch.");
				}

				// Advance to the next line
				i += 1;

				// Try parsing a block associated with this line.
				var gll = gll_core (i, depth.Value + 1);
				var block = gll.a;
				i = gll.b;

				rv.Add(new Block (filename, number, rest, block));
			}

			return new Tuple<List<Block>, int> (rv, i);
		}

		/// <summary>
		/// This takes as input the list of logical line triples output from
		/// list_logical_lines, and breaks the lines into blocks. Each block
		/// is represented as a list of (filename, line number, line text,
		/// block) triples, where block is a block list (which may be empty if
		/// no block is associated with this line.)
		/// </summary>
		/// <param name="lines">The lines to group into blocks.</param>
		private List<Block> group_logical_lines (List<Triple> lines)
		{
			return gll_core (0, 0).a;
		}

		#region Parse // Functions called to parse things.

		/// <summary>
		/// This parses a Ren'Py statement. l is expected to be a Ren'Py lexer
		/// that has been advanced to a logical line. This function will
		/// advance l beyond the last logical line making up the current
		/// statement, and will return an AST object representing this
		/// statement, or a list of AST objects representing this statement.
		/// </summary>
		private AST parse_statement (Lexer l)
		{
			var loc = l.get_location ();

			var pf = statements.parse (l);

			if (pf == null) {
				l.error ("expected statement");
			}

			return pf (l, loc);
		}

		/// <summary>
		/// This parses a block of Ren'Py statements. It returns a list of the
		/// statements contained within the block. l is a new Lexer object, for
		/// this block.
		/// </summary>
		private List<AST> parse_block (Lexer l)
		{
			l.advance ();

			var rv = new List<AST> ();

			while (!l.eob)
			{
				try {
					var stmt = parse_statement (l);

					// TODO: original source uses rv.extend if there is more
					// than one element; does that matter here?
					rv.Add (stmt);
				}
				catch (ParseError e) {
					parse_errors.Add (e);
					l.advance ();
				}
			}

			return rv;
		}

		/// <summary>
		/// Parses a Ren'Py script into a list of AST objects.
		/// </summary>
		/// <returns>
		/// A list of AST objects representing the statements that were found
		/// at the top level of the file.
		/// </returns>
		/// <param name="filename">
		/// The absolute path to the file.
		/// </param>
		/// <param name="filedata">
		/// A unicode string giving the file contents.
		/// </param>
		/// <param name="linenumber">
		/// If given, the parse starts at that line number.
		/// </param>
		public List<AST> Parse (string filename, string filedata, int linenumber = 1)
		{
			var lines = list_logical_lines (filename, filedata, linenumber);
			var nested = group_logical_lines (lines);

			var l = new Lexer (nested);
			var rv = parse_block (l);

//			if (null != rv) {
//				rv.append (ast.Return ((rv[-1].filename, rv[-1].linenumber), null))
//			}

			return rv;
		}

		#endregion
	}
}