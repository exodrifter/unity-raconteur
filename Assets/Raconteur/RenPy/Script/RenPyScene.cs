using UnityEngine;
using DPek.Raconteur.RenPy.Dialog;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyScene : RenPyStatement
	{
		private string m_imageName;

		public RenPyScene(ref RenPyScanner tokens) : base(RenPyStatementType.SCENE)
		{
			tokens.Seek("scene");
			tokens.Next();

			m_imageName = tokens.Seek(new string[] { "\n", "with" }).Trim();

			// Check if there is a "with" token next and ignore it
			if (tokens.PeekIgnoreWhitespace(true, true, true) == "with") {
				tokens.Seek("with");
				tokens.Seek("\n"); // Ignore the argument for with
				tokens.Next();
			}
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Remove all images
			display.State.RemoveAllImages();

			if (m_imageName == "black") {
				display.State.BackgroundImage = null;
				return;
			}

			string filename = display.State.GetImageFilename(m_imageName);
			Texture2D tex = display.RenPyScript.GetImage(filename);
			var image = new RenPyDialogImage(ref tex, RenPyAlignment.Center);

			display.State.BackgroundImage = image;
		}

		public override string ToString()
		{
			string str = "scene";
			str += " \"" + m_imageName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
