using System.Collections;

namespace DPek.Raconteur.RenPy.Parser
{
	/// <summary>
	/// Represents the definition of a token.
	/// </summary>
	public class TokenDefinition
	{

		#region Properties

		/// <summary>
		/// The sequence of characters to consider as a token.
		/// </summary>
		private char[] m_sequence;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new TokenDefinition that matches the specified character.
		/// </summary>
		/// <param name="ch"></param>
		public TokenDefinition(char ch)
		{
			m_sequence = new char[] { ch };
		}

		/// <summary>
		/// Creates a new TokenDefinition that matches the specified sequence of
		/// characters.
		/// </summary>
		/// <param name="sequence">
		/// A character array that represents the token to match.
		/// </param>
		public TokenDefinition(char[] sequence)
		{
			m_sequence = sequence;
		}

		/// <summary>
		/// Creates a new TokenDefinition that matches the specified string.
		/// </summary>
		/// <param name="token">
		/// A string that represents the token to match.
		/// </param>
		public TokenDefinition(string token)
		{
			m_sequence = token.ToCharArray();
		}

		#endregion

		#region Member Methods

		/// <summary>
		/// Checks if the character array contains the sequence of characters
		/// that this TokenDefinition matches starting at the specified index.
		/// </summary>
		/// <param name="index">
		/// The index to start checking in the character array.
		/// </param>
		/// <param name="chars">
		/// The array of characters to check in.
		/// </param>
		/// <param name="token">
		/// Set to this TokenDefinition's sequence of characters if this method
		/// returns true, otherwise it is set to null.
		/// </param>
		/// <returns>
		/// True if the sequence of characters starting at the specified index
		/// matches this TokenDefinition.
		/// </returns>
		public bool HasSequence(ref int index, ref char[] chars,
		                        out string token)
		{
			token = "";
			for (int offset = 0; offset < m_sequence.Length; ++offset) {
				if (m_sequence[offset] != chars[index + offset]) {
					token = null;
					return false;
				}

				token += m_sequence[offset];
			}

			index += m_sequence.Length - 1;
			return true;
		}

		#endregion
	}
}
