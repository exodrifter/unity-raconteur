using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.RenPy.Util;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py scene statement.
	/// </summary>
	public class RenPyScene : RenPyStatement
	{
		private string m_imageName;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyScene(ref Scanner tokens) : base(RenPyStatementType.SCENE)
		{
			tokens.Seek("scene");
			tokens.Next();

			m_imageName = tokens.Seek(new string[] { "\n", "with" }).Trim();

			// Check if there is a "with" token next and ignore it
			if (tokens.PeekIgnore(new string[]{" ","\t","\n"}) == "with") {
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
			Texture2D tex = state.Script.GetImage(filename);
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
