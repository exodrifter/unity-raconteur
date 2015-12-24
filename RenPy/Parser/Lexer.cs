using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Exodrifter.Raconteur.RenPy.Util;

using re = Exodrifter.Raconteur.RenPy.Util.PythonRegex;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// The Lexer is a class from renpy/parser.py
	/// 
	/// The lexer that is used to lex script files. This works on the idea
	/// that we want to lex each line in a block individually, and use
	/// sub-lexers to lex sub-blocks.
	/// </summary>
	public class Lexer
	{
		#region Constants

		/// <summary>
		/// A list of keywords which should not be parsed as names, because
		/// there is a huge chance of confusion.
		/// 
		/// Note: We need to be careful with what's in here, because thse
		/// are banned in simple_expressions, where we might want to use
		/// some of them.
		/// </summary>
		private static readonly List<string> KEYWORDS = new List<string> () {
			"$",
			"as",
			"at",
			"behind",
			"call",
			"expression",
			"hide",
			"if",
			"in",
			"image",
			"init",
			"jump",
			"menu",
			"onlayer",
			"python",
			"return",
			"scene",
			"set",
			"show",
			"with",
			"while",
			"zorder",
			"transform",
		};

		private static readonly List<string> OPERATORS = new List<string> () {
			"<",
			"<=",
			">",
			">=",
			"<>",
			"!=",
			"==",
			"|",
			"^",
			"&",
			"<<",
			">>",
			"+",
			"-",
			"*",
			"/",
			"//",
			"%",
			"~",
			"**",
		};

		private static readonly List<string> ESCAPED_OPERATORS = new List<string> () {
			"\\bor\\b",
			"\\band\\b",
			"\\bnot\\b",
			"\\bin\\b",
			"\\bis\\b",
		};

		/// <summary>
		/// Helper method for making the operator_regexp constant.
		/// </summary>
		private static string[] GetOperators ()
		{
			var ret = new List<string> (OPERATORS);
			ret.ForEach ( (x) => { x = re.escape (x); } );
			ret.AddRange (ESCAPED_OPERATORS);
			return ret.ToArray ();
		}

		private static readonly string operator_regexp
			= string.Join ("|", GetOperators ());

		private static readonly string word_regexp
			= "[a-zA-Z_\\u00a0-\\ufffd][0-9a-zA-Z_\\u00a0-\\ufffd]*";

		#endregion

		private bool init;

		private List<Line> block;
		public bool eob { get; private set; }

		private int line;

		private string filename;
		private string text;
		private int number;
		private List<Line> subblock;
		private int pos;
		private int word_cache_pos;
		private int word_cache_newpos;
		private string word_cache;

		/// <summary>
		/// The lexer that is used to lex script files. This works on the idea
		/// that we want to lex each line in a block individually, and use
		/// sub-lexers to lex sub-blocks.
		/// </summary>
		public Lexer (List<Line> block, bool init = false)
		{
			// Are we underneath an init block?
			this.init = init;

			this.block = block;
			this.eob = false;

			this.line = -1;

			// These are set by advance.
			this.filename = "";
			this.text = "";
			this.number = 0;
			this.subblock = new List<Line> ();
			this.pos = 0;
			this.word_cache_pos = -1;
			this.word_cache_newpos = -1;
			this.word_cache = "";
		}

		/// <summary>
		///  Advances this lexer to the next line in the block. The lexer
		/// starts off before the first line, so advance must be called
		/// before any matching can be done. Returns True if we've
		/// successfully advanced to a line in the block, or False if we
		/// have advanced beyond all lines in the block. In general, once
		/// this method has returned False, the lexer is in an undefined
		/// state, and it doesn't make sense to call any method other than
		/// advance (which will always return False) on the lexer.
		/// </summary>
		public bool advance ()
		{
			line += 1;

			if (line >= block.Count) {
				eob = true;
				return false;
			}

			filename = block[line].filename;
			number = block[line].number;
			text = block[line].text;
			subblock = block[line].block;
			pos = 0;
			word_cache_pos = -1;

			return true;
		}

		/// <summary>
		/// Tries to match the given regexp at the current location on the
		/// current line. If it succeds, it returns the matched text (if
		/// any), and updates the current position to be after the
		/// match. Otherwise, returns None and the position is unchanged.
		/// </summary>
		private string match_regexp (string regexp)
		{
			if (eob)
				return null;

			if (pos == text.Length)
				return null;

			var m = re.compile (regexp, re.DOTALL).Matches (text, pos);

			if (m.Count == 0)
				return null;

			// TODO: Make sure this is the equivalent of self.pos = m.end()
			pos += m[0].Length;

			return m[0].Value;
		}

		/// <summary>
		/// Advances the current position beyond any contiguous whitespace.
		/// </summary>
		private void skip_whitespace ()
		{
			match_regexp ("(\\s+|\\\\\\n)+");
		}

		/// <summary>
		/// Matches something at the current position, skipping past
		/// whitespace. Even if we can't match, the current position is
		/// still skipped past the leading whitespace.
		/// </summary>
		private string match (string regexp)
		{
			skip_whitespace ();
			return match_regexp (regexp);
		}

		/// <summary>
		/// Matches a keyword at the current position. A keyword is a word
		/// that is surrounded by things that aren't words, like
		/// whitespace. (This prevents a keyword from matching a prefix.)
		/// </summary>
		private string keyword (string word)
		{
			var oldpos = pos;
			if (this.word () == word)
				return word;

			pos = oldpos;
			return "";
		}

		/// <summary>
		/// Convenience function for reporting a parse error at the current
		/// location.
		/// </summary>
		public void error (string msg)
		{
			throw new ParseError (filename, number, msg, text, pos);
		}

		/// <summary>
		/// Returns True if, after skipping whitespace, the current
		/// position is at the end of the end of the current line, or
		/// False otherwise.
		/// </summary>
		private bool eol ()
		{
			skip_whitespace ();
			return pos >= text.Length;
		}

		/// <summary>
		/// If we are not at the end of the line, raise an error.
		/// </summary>
		private void expect_eol ()
		{
			if (!eol())
				error ("end of line expected.");
		}

		/// <summary>
		/// Called to indicate that the statement requires that a non-empty
		/// block is present.
		/// </summary>
		private void expect_noblock (string stmt)
		{
			if (subblock.Count == 0)
				error (string.Format ("{0} expects a non-empty block.", stmt));
		}

		/// <summary>
		/// Returns a new lexer object, equiped to parse the block
		/// associated with this line.
		/// </summary>
		private Lexer subblock_lexer (bool init)
		{
			init = this.init || init;

			return new Lexer (subblock, init);
		}

		/// <summary>
		/// Lexes a string, and returns the string to the user, or none if
		/// no string could be found. This also takes care of expanding
		/// escapes and collapsing whitespace.
		/// 
		/// Be a little careful, as this can return an empty string, which is
		/// different than None.
		/// </summary>
		private string @string ()
		{
			var s = match ("r?\"([^\\\\\"]|\\\\.)*\"");

			if (s == null)
				s = match ("r?'([^\\\\']|\\\\.)*'");

			if (s == null)
				s = match ("r?`([^\\\\`]|\\\\.)*`");

			if (s == null)
				return null;

			bool raw = false;
			if (s[0] == 'r') {
				raw = true;
				s = s.Slice (1, null);
			}

			// Strip off delimiters.
			s = s.Slice(1, -1);

			if (!raw)
			{
				// Collapse runs of whitespace into single spaces.
				s = re.sub ("\\s+", " ", s);

				s = s.Replace ("\\n", "\n");
				s = s.Replace ("\\{", "{{");
				s = s.Replace ("\\[", "[[");
				s = s.Replace ("\\%", "%%");
				// TODO: Make this work
				//s = re.sub("\\\\u([0-9a-fA-F]{1,4})",
				//	(MatchCollection m) => unichr (int (m[1], 16)), s)
				s = re.sub("\\\\(.)", "\\1", s);
			}

			return s;
		}

		/// <summary>
		/// Tries to parse an integer. Returns a string containing the
		/// integer, or None.
		/// </summary>
		private string integer ()
		{
			return match ("(\\+|\\-)?\\d+");
		}

		/// <summary>
		/// Tries to parse a number (float). Returns a string containing the
		/// number, or None.
		/// </summary>
		private string @float ()
		{
			return match ("(\\+|\\-)?(\\d+\\.?\\d*|\\.\\d+)([eE][-+]?\\d+)?");
		}

		/// <summary>
		/// Matches the chatacters in an md5 hash, and then some.
		/// </summary>
		private string hash ()
		{
			return match("\\w+");
		}

		/// <summary>
		/// Parses a name, which may be a keyword or not.
		/// </summary>
		private string word ()
		{
			if (pos == word_cache_pos) {
				pos = word_cache_newpos;
				return word_cache;
			}

			word_cache_pos = pos;
			var rv = match (word_regexp);
			word_cache = rv;
			word_cache_newpos = pos;

			return rv;
		}

		/// <summary>
		/// This tries to parse a name. Returns the name or None.
		/// </summary>
		private string name ()
		{
			var oldpos = pos;
			var rv = word();

			if (rv.In (KEYWORDS)) {
				pos = oldpos;
				return null;
			}

			return rv;
		}

		/// <summary>
		/// This tries to match a python string at the current
		/// location. If it matches, it returns True, and the current
		/// position is updated to the end of the string. Otherwise,
		/// returns False.
		/// </summary>
		private bool python_string ()
		{
			if (eol ())
				return false;

			var c = text[pos];

			// Allow unicode strings.
			if (c == 'u') {
				pos += 1;

				if (pos == text.Length) {
					pos -= 1;
					return false;
				}

				c = text[pos];

				if (!c.In ('\"', '\'')) {
					pos -= 1;
					return false;
				}
			}
			else if (!c.In ('\"', '\'')) {
				return false;
			}

			var delim = c;

			while (true)
			{
				pos += 1;

				if (eol ())
					error ("end of line reached while parsing string.");

				c = text[pos];

				if (c == delim)
					break;

				if (c == '\\')
					pos += 1;
			}

			pos += 1;
			return true;
		}

		/// <summary>
		/// This tries to match a dotted name, which is one or more names,
		/// separated by dots. Returns the dotted name if it can, or None
		/// if it cannot.
		/// 
		/// Once this sees the first name, it commits to parsing a
		/// dotted_name. It will report an error if it then sees a dot
		/// without a name behind it.
		/// </summary>
		private string dotted_name ()
		{
			var rv = name ();

			if (rv == null) {
				return null;
			}

			while (match ("\\.") != null) {
				var n = name  ();
				if (n == null)
					error ("expecting name.");

				rv += "." + n;
			}

			return rv;
		}

		/// <summary>
		/// This matches python code up to, but not including, the
		/// non-whitespace delimiter characters. Returns a string containing
		/// the matched code, which may be empty if the first thing is the
		/// delimiter. Raises an error if EOL is reached before the delimiter.
		/// </summary>
		private PyExpr delimited_python (params char[] delim)
		{
			var start = pos;

			while (!eol ())
			{
				var c = text[pos];

				if (c.In (delim)) {
					return new PyExpr (text.Slice (start, pos), filename, number);
				}

				if (c == '\"' || c == '\'') {
					python_string ();
					continue;
				}

				if (parenthesised_python ())
					continue;

				pos += 1;
			}

			error (string.Format ("reached end of line when expecting '{0}'.", delim));
			return null;
		}

		/// <summary>
		/// Returns a python expression, which is arbitrary python code
		/// extending to a colon.
		/// </summary>
		private PyExpr python_expression ()
		{
			var pe = delimited_python (':');

			if (pe == null)
				error ("expected python_expression");

			var rv = new PyExpr (pe.str.Strip (), pe.filename, pe.linenumber); // E1101

			return rv;
		}

		/// <summary>
		/// Tries to match a parenthesised python expression. If it can,
		/// returns true and updates the current position to be after the
		/// closing parenthesis. Returns False otherewise.
		/// </summary>
		private bool parenthesised_python ()
		{
			var c = text[pos];

			if (c == '(') {
				pos += 1;
				delimited_python (')');
				pos  += 1;
				return true;
			}

			if (c == '[') {
				pos += 1;
				delimited_python (']');
				pos  += 1;
				return true;
			}

			if (c == '{') {
				pos += 1;
				delimited_python ('}');
				pos  += 1;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Tries to parse a simple_expression. Returns the text if it can, or
		/// None if it cannot.
		/// </summary>
		private PyExpr simple_expression (bool comma = false)
		{
			var start = pos;

			// Operator.
			while (true)
			{
				while (match (operator_regexp) != null)
					continue;

				if (eol ())
					break;

				// We start with either a name, a python_string, or
				// parenthesized python
				if (!python_string () ||
					name () != null ||
					@float() != null ||
					parenthesised_python())

					break;

				while (true)
				{
					skip_whitespace ();

					if (eol ())
						break;

					// If we see a dot, expect a dotted name.
					if (match ("\\.") != null) {
						var n = word();
						if (n == null)
							error("expecting name after dot.");

						continue;
					}

					// Otherwise, try matching parenthesised python.
					if (parenthesised_python ())
						continue;

					break;
				}

				if (match (operator_regexp) != null)
					continue;

				if (comma && (match (",") != null))
					continue;

				break;
			}

			text = text.Slice (start, pos).Strip ();

			if (text == null || text.Length == 0) {
				return null;
			}

			return new PyExpr (text.Slice (start, pos).Strip(), filename, number);
		}

		/// <summary>
		/// One or more simple expressions, separated by commas, including an
		/// optional trailing comma.
		/// </summary>
		private PyExpr comma_expression ()
		{
			return simple_expression (true);
		}

		public struct LexerState {
			public int line;
			public string filename;
			public int number;
			public string text;
			public List<Line> subblock;
			public int pos;

			public LexerState (int line, string filename, int number,
				string text, List<Line> subblock, int pos)
			{
				this.line = line;
				this.filename = filename;
				this.number = number;
				this.text = text;
				this.subblock = new List<Line> (subblock);
				this.pos = pos;
			}
		}

		/// <summary>
		/// Returns an opaque representation of the lexer state. This can be
		/// passed to revert to back the lexer up.
		/// </summary>
		public LexerState checkpoint ()
		{
			return new LexerState (line, filename, number, text, subblock, pos);
		}

		/// <summary>
		/// Reverts the lexer to the given state. State must have been returned
		/// by a previous checkpoint operation on this lexer.
		/// </summary>
		public void revert (LexerState state)
		{
			line = state.line;
			filename = state.filename;
			number = state.number;
			text = state.text;
			subblock = new List<Line> (subblock);
			pos = state.pos;
			word_cache_pos = -1;
		}

		/// <summary>
		/// Returns a (filename, line number) tuple representing the current
		/// physical location of the start of the current logical line.
		/// </summary>
		public Tuple<string, int> get_location ()
		{
			return new Tuple<string, int> (filename, number);
		}

		/// <summary>
		/// Tries to parse thing, and reports an error if it cannot be done.
		/// 
		/// If thing is a string, tries to parse it using
		/// self.match(thing). Otherwise, thing must be a method on this lexer
		/// object, which is called directly.
		/// </summary>
		private string require (string thing, string name = null)
		{
			name = name ?? thing;
			var rv = match (thing);

			if (rv == null)
				error (string.Format ("expected '{0}' not found.", name));

			return rv;
		}

		private string require<T> (MethodInfo thing, string name = null)
		{
			name = name ?? thing.Name;
			var rv = thing.Invoke (this, null) as string;

			if (rv == null)
				error (string.Format ("expected '{0}' not found.", name));

			return rv;
		}

		/// <summary>
		/// Skips whitespace, then returns the rest of the current
		/// line, and advances the current position to the end of
		/// the current line.
		/// </summary>
		private PyExpr rest ()
		{
			skip_whitespace ();

			var pos = this.pos;
			this.pos = text.Length;
			return new PyExpr (text.Slice (pos, null).Strip (), filename, number);
		}

		private List<string> process (List<Line> block, string indent, ref int line)
		{
			var rv = new List<string> ();

			foreach (var l in block)
			{
				var ln = l.number;

				while (line < ln) {
					rv.Add (indent + '\n');
					line += 1;
				}

				var linetext = indent + text + '\n';

				rv.Add (linetext);
				line += linetext.Count ('\n');

				rv.AddRange (process (subblock, indent + "    ", ref line));
			}

			return rv;
		}

		/// <summary>
		/// Returns the subblock of this code, and subblocks of that
		/// subblock, as indented python code. This tries to insert
		/// whitespace to ensure line numbers match up.
		/// </summary>
		private string python_block ()
		{
			var line = number;
			var rv = process (subblock, "", ref line);
			return string.Join ("", rv.ToArray ());
		}
	}
}
