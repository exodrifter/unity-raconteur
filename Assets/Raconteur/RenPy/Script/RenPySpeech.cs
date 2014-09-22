using UnityEngine;
using System.Text.RegularExpressions;

using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPySpeech : RenPyLine
	{
		private string m_character;
		public string Character
		{
			get {
				return m_character;
			}
		}

		private string m_text;
		public string Text
		{
			get {
				return m_text;
			}
		}

		public RenPySpeech(ref RenPyScanner tokens) : base(RenPyLineType.SPEECH)
		{
			m_character = tokens.Seek("\"").Trim();
			tokens.Next();
			m_text = tokens.Seek("\"").Replace("\\\"", "\"");
			m_text = m_text.Replace("\n", ""); // Remove extra newlines
			Regex trimmer = new Regex(@"\s\s+"); // Remove extra whitespace
			m_text = trimmer.Replace(m_text, " ");
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Go to the next line if we are skipping the dialog
			if (Static.SkipDialog) {
				display.State.NextLine(display);
			}
		}

		public override string ToString()
		{
			string str = m_character;
			str += !string.IsNullOrEmpty(str) ? " " : "";
			str += "\"" + m_text + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
