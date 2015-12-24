using UnityEngine;
using System;
using System.Collections;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// ParseTrie is a class from renpy/parse.py
	/// 
	/// This is a trie of words, that's used to pick a parser function.
	/// </summary>
	public class ParseTrie : MonoBehaviour
	{
		public ParseTrie ()
		{
			// TODO: Implement
			throw new NotImplementedException ();
		}

		public void add ()
		{
			// TODO: Implement
			throw new NotImplementedException ();
		}

		public Func<Lexer, Tuple<string, int>, AST> parse (Lexer l)
		{
			// TODO: Implement
			throw new NotImplementedException ();
		}
	}

	public class AST
	{
		// TODO: Implement
	}
}
