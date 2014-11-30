using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Parser
{
	/// <summary>
	/// A scanner for iterating through a list of strings or tokens.
	/// </summary>
	public class RenPyScanner
	{
		/// <summary>
		/// The list of tokens.
		/// </summary>
		private LinkedList<string> m_tokens;

		/// <summary>
		/// The current token that this scanner is pointing at.
		/// </summary>
		private LinkedListNode<string> m_node;

		/// <summary>
		/// Creates a scanner with the passed tokens.
		/// </summary>
		/// <param name="tokens">
		/// A reference to a list of tokens.
		/// </param>
		public RenPyScanner(ref LinkedList<string> tokens)
		{
			this.m_tokens = tokens;
			this.m_node = m_tokens.First;
		}

		/// <summary>
		/// Determines whether this scanner has another token after the current
		/// token.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this scanner has a next token; otherwise,
		/// <c>false</c>.
		/// </returns>
		public bool HasNext()
		{
			return m_node != null && m_node.Next != null;
		}

		/// <summary>
		/// Returns the current token.
		/// </summary>
		public string Peek()
		{
			return m_node.Value;
		}

		/// <summary>
		/// Returns the next token in this scanner that isn't one of the ignored
		/// tokens. Specifically, the scanner will search from and including the
		/// current token until it finds a token that is not one of the ignored
		/// tokens.
		/// </summary>
		/// <returns>
		/// The next token in this scanner that isn't one of the ignored tokens.
		/// </returns>
		/// <param name="ignoredTokens">
		/// The tokens to ignore.
		/// </param>
		public string PeekIgnore(string[] ignoredTokens)
		{
			LinkedListNode<string> node = m_node;
			while (node != null) {
				string str = node.Value;
				foreach(var ignoredToken in ignoredTokens) {
					if (str == ignoredToken) {
						node = node.Next;
						continue;
					}
				}
				break;
			}
			return node.Value;
		}

		/// <summary>
		/// Returns the token right before the next new line in this scanner.
		/// Specifically, the scanner will search from and including the current
		/// token until it finds a new line token ("\n"), upon which it will
		/// return the token right before the new line token.
		/// </summary>
		/// <returns>
		/// The token right before the next new line in this scanner.
		/// </returns>
		public string PeekEnd()
		{
			LinkedListNode<string> node = m_node;
			string endToken = null;
			while (node != null) {
				string str = node.Value;
				if (str == "\n") {
					break;
				}
				endToken = str;
				node = node.Next;
			}
			return endToken;
		}
		
		/// <summary>
		/// Returns the current token and moves this scanner to the next token.
		/// </summary>
		/// <returns>
		/// The current token.
		/// </returns>
		public string Next()
		{
			if (m_node == null) {
				return null;
			}
			string ret = m_node.Value;
			m_node = m_node.Next;
			return ret;
		}

		/// <summary>
		/// Moves this token to point to the next token that matches the
		/// specified string. Specifically, the scanner will search from and
		/// including the current token until it finds a token that matches the
		/// specified token, upon which the scanner will now point to that
		/// token. Then, this method returns the tokens that this scanner
		/// skipped over.
		/// </summary>
		/// <param name="token">
		/// The token to point to.
		/// </param>
		/// <returns>
		/// The tokens that are between the current token (inclusive) and the
		/// token to point to (exclusive).
		/// </returns>
		public string Seek(string token)
		{
			string skipped = "";
			while (m_node != null && m_node.Value != token) {
				skipped += m_node.Value;
				m_node = m_node.Next;
			}
			return skipped;
		}
		
		/// <summary>
		/// Moves this token to point to the next token that matches any of the
		/// specified strings. Specifically, the scanner will search from and
		/// including the current token until it finds a token that matches one
		/// of the specified tokens, upon which the scanner will now point to
		/// that token. Then, this method returns the tokens that this scanner
		/// skipped over.
		/// </summary>
		/// <param name="token">
		/// The token to point to.
		/// </param>
		/// <returns>
		/// The tokens that are between the current token (inclusive) and the
		/// token to point to (exclusive).
		/// </returns>
		public string Seek(string[] tokens)
		{
			string temp;
			return Seek(tokens, out temp);
		}
		
		/// <summary>
		/// Moves this token to point to the next token that matches any of the
		/// specified strings. Specifically, the scanner will search from and
		/// including the current token until it finds a token that matches one
		/// of the specified tokens, upon which the scanner will now point to
		/// that token. Then, this method returns the tokens that this scanner
		/// skipped over.
		/// </summary>
		/// <param name="token">
		/// The token to point to.
		/// </param>
		/// <param name="token">
		/// The token that was matched from the list of specified tokens.
		/// </param>
		/// <returns>
		/// The tokens that are between the current token (inclusive) and the
		/// token to point to (exclusive).
		/// </returns>
		public string Seek(string[] tokens, out string match)
		{
			string skipped = "";
			while (HasNext()) {
				foreach (string token in tokens) {
					if (m_node.Value == token) {
						match = token;
						return skipped;
					}
				}
				skipped += m_node.Value;
				m_node = m_node.Next;
			}
			match = null;
			return skipped;
		}

		/// <summary>
		/// Skips empty lines and returns the number of empty lines skipped.
		/// </summary>
		/// <returns>
		/// The number of skipped empty lines.
		/// </returns>
		public int SkipEmptyLines()
		{
			LinkedListNode<string> oldNode = m_node;
			int skipped = 0;

			string str = Seek("\n");
			while (string.IsNullOrEmpty(str.Trim())) {
				string result = Next();
				if (result == null) {
					// Seek did not find newline, we are at end of document.
					// Quit immediately.
					return skipped;
				}
				skipped++;
				oldNode = m_node;
				str = Seek("\n");
			}

			m_node = oldNode;
			return skipped;
		}

		/// <summary>
		/// Moves the scanner to point to the first next token that is not one
		/// of the ignored tokens. Specifically, the scanner will search from
		/// and including the current token until it finds a token that does not
		/// match one of the specified tokens, upon which the scanner will now
		/// point to that token. Then, this method returns the number of tokens
		/// that this scanner skipped over.
		/// </summary>
		/// <returns>
		/// The number of tokens the scanner skipped.
		/// </returns>
		/// <param name="ignoredTokens">
		/// The tokens to ignore.
		/// </param>
		public int Skip(string[] ignoredTokens)
		{
			int skipped = 0;
			while (HasNext()) {
				string str = Peek();
				bool ignore = false;

				foreach(string ignored in ignoredTokens) {
					if(str == ignored) {
						Next();
						skipped++;
						ignore = true;
						break;
					}
				}

				if(ignore) {
					continue;
				}

				break;
			}
			return skipped;
		}
	}
}
