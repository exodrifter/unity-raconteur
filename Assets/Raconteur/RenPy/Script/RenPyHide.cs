using UnityEngine;
using DPek.Raconteur.RenPy.Dialog;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyHide : RenPyStatement
	{
		private string m_imageName;

		public RenPyHide(ref RenPyScanner tokens) : base(RenPyStatementType.HIDE)
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

		public override void Execute(RenPyDisplayState display)
		{
			display.State.RemoveImage(m_imageName);
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
