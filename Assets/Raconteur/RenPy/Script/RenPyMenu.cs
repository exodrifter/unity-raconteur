﻿using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py menu statement.
	/// </summary>
	public class RenPyMenu : RenPyStatement
	{
		public Dictionary<string, string> m_choices;

		public RenPyMenu(ref RenPyScanner tokens)
			: base(RenPyStatementType.MENU)
		{
			m_choices = new Dictionary<string, string>();

			tokens.Seek("menu");
			tokens.Seek("\n");
			tokens.Next();

			while (true) {
				tokens.SkipEmptyLines();
				int spaces = tokens.SkipWhitespace(true, false, false);

				if (spaces == 8) {
					tokens.Seek("\"");
					tokens.Next();
					string choice = tokens.Seek("\"");

					tokens.Seek("jump");
					tokens.Next();
					string jump = tokens.Seek("\n").Trim();

					m_choices.Add(choice, jump);

					tokens.Next();
					continue;
				} else {
					break;
				}
			}
		}

		public override void Execute(RenPyState state)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "menu";
			foreach (KeyValuePair<string, string> item in m_choices) {
				str += " (\"" + item.Key + "\"=>" + item.Value + ")";
			}

			str += "\n" + base.ToString();
			return str;
		}
	}
}