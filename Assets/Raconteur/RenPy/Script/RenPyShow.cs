using UnityEngine;
using DPek.Raconteur.RenPy.Dialog;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyShow : RenPyStatement
	{
		private string m_imageName;
		private RenPyAlignment m_alignment;

		public RenPyShow(ref RenPyScanner tokens) : base(RenPyStatementType.SHOW)
		{
			tokens.Seek("show");
			tokens.Next();

			string[] arr = new string[] { "\n", "with", "at"};
			m_imageName = tokens.Seek(arr).Trim();
			tokens.SkipWhitespace(true, true, true);

			bool foundToken = true;
			while (foundToken)
			{
				foundToken = false;

				// Check if there is a "with" argument
				if (tokens.Peek() == "with")
				{
					tokens.Next();
					tokens.SkipWhitespace();
					tokens.Next(); // TODO: Don't ignore the with argument
					foundToken = true;
				}
				// Check if there is an "at" argument
				else if (tokens.Peek() == "at")
				{
					tokens.Next();
					tokens.SkipWhitespace();
					string alignment = tokens.Next();
					m_alignment = Util.RenPyAlignmentConverter.FromString(alignment);
					foundToken = true;
				}
				// TODO: Check for other arguments

				// Skip following whitespace
				tokens.SkipWhitespace(true, true, true);
			}
		}

		public override void Execute(RenPyDisplayState display)
		{
			string filename = display.State.GetImageFilename(m_imageName);
			Texture2D tex = display.RenPyScript.GetImage(filename);
			var image = new RenPyDialogImage(ref tex, m_alignment);

			display.State.AddImage(m_imageName, ref image);
		}

		public override string ToString()
		{
			string str = "show";
			str += " \"" + m_imageName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
