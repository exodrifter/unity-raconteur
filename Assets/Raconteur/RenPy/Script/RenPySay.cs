using UnityEngine;
using System.Text.RegularExpressions;

using DPek.Raconteur.RenPy.Display;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py say statement.
	/// </summary>
	public class RenPySay : RenPyStatement
	{
		/// <summary>
		/// The variable or string of the speaker saying this line. Null when
		/// the narrator is saying the line
		/// </summary>
		[SerializeField]
		private string m_speaker;

		/// <summary>
		/// Whether or not the speaker is a variable or a string.
		/// </summary>
		[SerializeField]
		private bool m_speakerIsVariable;

		/// <summary>
		/// The text of what the character is saying
		/// </summary>
		[SerializeField]
		private string m_text;
		public string Text
		{
			get {
				return m_text;
			}
		}

		public RenPySay() : base(RenPyStatementType.SAY)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			string[] quotes = new string[] {"\"","'"};

			// Get the contents up to the next open quote
			string endQuote;
			m_speaker = tokens.Seek(quotes, out endQuote).Trim();
			tokens.Next();

			// If contents are empty, then we have a character string OR a
			// narrator-spoken line
			if (string.IsNullOrEmpty(m_speaker)) {
				m_speakerIsVariable = false;
				m_speaker = tokens.Seek(endQuote).Trim();
				tokens.Next();

				// Check if we have a narrated line
				tokens.Seek(new string[] { "\n", "\"", "'" }, out endQuote);
				if (endQuote == "\n") {
					m_text = ProcessText(m_speaker);
					m_speaker = null;
					return;
				}

				// Otherwise, this is a character string
				tokens.Next();
			}

			// Otherwise, we already have the character variable
			else {
				m_speakerIsVariable = true;
				if (string.IsNullOrEmpty(m_speaker)) {
					m_speaker = null;
				}
			}

			// Now, get rest of the text
			m_text = ProcessText(tokens.Seek(endQuote));
			tokens.Next();
		}

		private string ProcessText(string text) {
			text = text.Replace("\\\"", "\"").Replace("\\'", "'");
			text = text.Replace("\n", ""); // Remove extra newlines
			Regex trimmer = new Regex(@"\s\s+"); // Remove extra whitespace
			text = trimmer.Replace(text, " ");
			return text;
		}

		public override void Execute(RenPyState state)
		{
			// Stop requesting the dialog window
			state.Visual.WindowRequested = false;
		}

		public RenPyCharacterData GetSpeaker(RenPyState state)
		{
			if (m_speakerIsVariable) {
				RenPyCharacter ch = state.GetCharacter(m_speaker);
				return new RenPyCharacterData(ch.Name, ch.Color);
			}
			else {
				return new RenPyCharacterData(m_speaker, Color.white);
			}
		}

		public override string ToDebugString()
		{
			string str = m_speaker;
			str += !string.IsNullOrEmpty(str) ? " " : "";
			str += "\"" + m_text + "\"";
			return str;
		}
	}
}
