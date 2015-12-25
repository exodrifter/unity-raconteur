using UnityEngine;
using System;
using System.Collections.Generic;
using Exodrifter.Raconteur.RenPy.Util;

using Loc = Exodrifter.Raconteur.RenPy.Util.Tuple<string, int>;
using ParseFunction = System.Func<Exodrifter.Raconteur.RenPy.Lexer, Exodrifter.Raconteur.RenPy.Util.Tuple<string, int>, Exodrifter.Raconteur.RenPy.Util.Node>;

namespace Exodrifter.Raconteur.RenPy.Util
{
	/// <summary>
	/// ParseTrie is a translation of a class with the same name from
	/// renpy/parse.py. The purpose of ParseTrie is to select the correct
	/// parsing method for each statement.
	/// </summary>
	public class ParseTrie : MonoBehaviour
	{
		private readonly Parser parser;

		private Dictionary<string, ParseFunction> parseFunctions;

		public ParseTrie (Parser parser)
		{
			this.parser = parser;
			parseFunctions = new Dictionary<string, ParseFunction> ();

			add ("if", if_statement);
		}

		public void add (string keyword, ParseFunction fn)
		{
			// TODO: Implement
			throw new NotImplementedException ();
		}

		public Func<Lexer, Tuple<string, int>, Node> parse (Lexer l)
		{
			// TODO: Implement
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Convenience method for calling parser.parse_block(Lexer)
		/// </summary>
		private List<Node> parse_block (Lexer l)
		{
			return parser.parse_block (l);
		}

		private Node if_statement (Lexer l, Loc loc)
		{
			var entries = new List<Tuple<PyExpr, List<Node>>> ();

			var condition = l.require<PyExpr> (l.python_expression);
			l.require (":");
			l.expect_eol ();
			l.expect_block ("if statement");

			var block = parse_block (l.subblock_lexer());

			entries.Add (new Tuple<PyExpr, List<Node>> (condition, block));

			l.advance ();

			while (l.keyword("elif") != null)
			{
				condition = l.require<PyExpr> (l.python_expression);
				l.require (":");
				l.expect_eol ();
				l.expect_block ("elif clause");

				block = parse_block(l.subblock_lexer());

				entries.Add (new Tuple<PyExpr, List<Node>> (condition, block));

				l.advance ();
			}

			if (l.keyword("else") != null)
			{
				l.require (":");
				l.expect_eol ();
				l.expect_block ("else clause");

				block = parse_block (l.subblock_lexer ());

				condition = new PyExpr ("True", null, 0); // TODO: Two of these arguments are incorrect
				entries.Add (new Tuple<PyExpr, List<Node>>(condition, block));

				l.advance ();
			}

			return new IfNode (loc, entries);
		}
	}

	/// <summary>
	/// An AST node that represents a Ren'Py statement.
	/// </summary>
	public class Node
	{
		// TODO: Implement
	}

	/// <summary>
	/// An AST node that represents a Ren'Py if statement.
	/// </summary>
	public class IfNode : Node
	{
		public IfNode (Loc loc, List<Tuple<PyExpr, List<Node>>> entries)
		{
			
		}
	}
}
