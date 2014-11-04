using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py scene statement.
	/// </summary>
	public class RenPyScene : RenPyStatement
	{
		[SerializeField]
		private string m_imageName;

		public RenPyScene() : base(RenPyStatementType.SCENE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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

		public override void Execute(RenPyState state)
		{
			// Remove all images
			state.Visual.RemoveAllImages();

			if (m_imageName == "black") {
				state.Visual.SetBackgroundImage(null);
				return;
			}

			string filename = state.GetImageFilename(m_imageName);
			Texture2D tex = state.Data.GetImage(filename);
			var image = new RenPyImageData(ref tex, RenPyAlignment.Center);

			state.Visual.SetBackgroundImage(image);
		}

		public override string ToDebugString()
		{
			string str = "scene";
			str += " \"" + m_imageName + "\"";
			return str;
		}
	}
}
