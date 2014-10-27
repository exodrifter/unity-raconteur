﻿using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py hide statement.
	/// </summary>
	public class RenPyHide : RenPyStatement
	{
		/// <summary>
		/// The name of the image to hide
		/// </summary>
		private string m_imageName;

		public RenPyHide(ref RenPyScanner tokens)
			: base(RenPyStatementType.HIDE)
		{
			tokens.Seek("hide");
			tokens.Next();

			string[] arr = new string[] { "\n", "with" };
			m_imageName = tokens.Seek(arr).Trim();
			tokens.SkipWhitespace(true, true, true);

			bool foundToken = true;
			while (foundToken)
			{
				foundToken = false;

				// Check if there is a "with" argument
				if (tokens.PeekIgnoreWhitespace(true, true, true) == "with")
				{
					tokens.SkipWhitespace(true, true, true);
					tokens.Next();
					tokens.SkipWhitespace();
					tokens.Next(); // TODO: Don't ignore the with argument
					foundToken = true;
				}

				// TODO: Check for other arguments
			}
		}

		public override void Execute(RenPyState state)
		{
			state.Visual.RemoveImage(m_imageName);
		}

		public override string ToString()
		{
			string str = "hide";
			str += " \"" + m_imageName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}