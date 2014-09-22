using System.Collections.Generic;

namespace DPek.Raconteur.RenPy.Parser
{
	/// <summary>
	/// A class that can produce a sequence of tokens from a string that
	/// represents Twine source code.
	/// </summary>
	public class Tokenizer
	{
		#region Properties

		/// <summary>
		/// A list of token definitions that this Tokenizer will attempt to
		/// match.
		/// </summary>
		private List<TokenDefinition> m_tokenDefinitions;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new Tokenizer with the default tokens for RenPy.
		/// </summary>
		public Tokenizer()
		{
			m_tokenDefinitions = new List<TokenDefinition>();

			// Setup parsing for Twine tokens
			string[] tokens;
			tokens = "[ ] ( ) # \\\" \" ' ; = + - * / \\ : $".Split(' ');
			foreach (string str in tokens) {
				m_tokenDefinitions.Add(new TokenDefinition(str));
			}
		}

		#endregion

		#region Member Methods

		/// <summary>
		/// Tokenizes a string by identifying TokenDefinitions and seperating by
		/// whitespace. Treats newlines as a token.
		/// </summary>
		/// <param name="str">
		/// The string to tokenize
		/// </param>
		/// <returns>
		/// An array of tokens generated from the passed string.
		/// </returns>
		public string[] Tokenize(ref string str)
		{
			List<string> tokens = new List<string>();
			char[] chars = str.ToCharArray();
			int index = -1;
			int length = chars.Length;

			string currentToken = "";
			bool newlineAdded = false;

			while (index < length - 1) {
				// Get the next character
				index++;
				char ch = chars[index];

				// Check for whitespace
				if (ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r') {

					// Add the current token
					if (AddToken(ref currentToken, ref tokens)) {
						newlineAdded = false;
					}

					// Treat whitespace as a token
					if (ch == ' ') {
						tokens.Add(" ");
					}

					// Treat tabs as a token
					else if (ch == '\t') {
						tokens.Add("\t");
					}

					// Treat '\n' as a token, unless the last token was '\n'
					else if ((ch == '\n' || ch == '\r') && !newlineAdded) {
						tokens.Add("\n");
						newlineAdded = true;
					}

					// Start the loop over
					continue;
				}

				// Check for token definitions
				bool broke = false;
				foreach (TokenDefinition def in m_tokenDefinitions) {
					string token;
					if (def.HasSequence(ref index, ref chars, out token)) {

						// Add the current token
						if (AddToken(ref currentToken, ref tokens)) {
							newlineAdded = false;
						}

						// Add the found token definition
						tokens.Add(token);

						// Start the loop over
						broke = true;
						break;
					}
				}
				if (broke) {
					continue;
				}

				// If no special cases were found, build the current token
				currentToken += ch;
			}

			// Add leftover token
			AddToken(ref currentToken, ref tokens);

			return tokens.ToArray();
		}

		/// <summary>
		/// Adds a string to a list of tokens and resets the string back to an
		/// empty string if it is not already empty string.
		/// </summary>
		/// <param name="token">
		/// The token to add.
		/// </param>
		/// <param name="tokens">
		/// The token list to add the token to
		/// </param>
		/// <returns>
		/// True if the token was added
		/// </returns>
		private bool AddToken(ref string token, ref List<string> tokens)
		{
			if (token != "") {
				tokens.Add(token);
				token = "";
				return true;
			}
			return false;
		}

		#endregion
	}
}