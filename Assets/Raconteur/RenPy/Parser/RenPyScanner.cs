using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Parser
{
	public class RenPyScanner
	{
		private LinkedList<string> m_tokens;
		private LinkedListNode<string> m_node;

		public RenPyScanner(ref LinkedList<string> tokens)
		{
			this.m_tokens = tokens;
			this.m_node = m_tokens.First;
		}

		public bool HasNext()
		{
			return m_node != null && m_node.Next != null;
		}

		public string Peek()
		{
			return m_node.Value;
		}
		
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

		public string PeekIgnoreWhitespace(bool spaces = true,
		                                   bool tabs = true,
		                                   bool newLines = false)
		{
			LinkedListNode<string> node = m_node;
			while (node != null) {
				string str = node.Value;
				if ((spaces && str == " ") || (tabs && str == "\t")
				    || (newLines && str == "\n")) {
					node = node.Next;
					continue;
				}
				break;
			}
			return node.Value;
		}

		public string Next()
		{
			if (m_node == null) {
				return null;
			}
			string ret = m_node.Value;
			m_node = m_node.Next;
			return ret;
		}

		public string Seek(string token)
		{
			string skipped = "";
			while (m_node != null && m_node.Value != token) {
				skipped += m_node.Value;
				m_node = m_node.Next;
			}
			return skipped;
		}

		public string Seek(string[] tokens)
		{
			string temp;
			return Seek(tokens, out temp);
		}

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

		public int SkipEmptyLines()
		{
			LinkedListNode<string> oldNode = m_node;
			int skipped = 0;

			string str = Seek("\n");
			while (string.IsNullOrEmpty(str)) {
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

		public int SkipWhitespace(bool spaces = true,
		                          bool tabs = true,
		                          bool newLines = false)
		{
			int skipped = 0;
			while (HasNext()) {
				string str = Peek();
				if ((spaces && str == " ") || (tabs && str == "\t")
				    || (newLines && str == "\n")) {
					Next();
					skipped++;
					continue;
				}
				break;
			}
			return skipped;
		}
	}
}
