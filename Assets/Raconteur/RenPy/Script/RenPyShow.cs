using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py show statement.
	/// </summary>
	public class RenPyShow : RenPyStatement
	{
		[SerializeField]
		private string m_imageName;
		[SerializeField]
		private RenPyAlignment m_alignment;

		public RenPyShow() : base(RenPyStatementType.SHOW)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("show");
			tokens.Next();

			string[] arr = new string[] { "\n", "with", "at"};
			m_imageName = tokens.Seek(arr).Trim();

			bool foundToken = true;
			while (foundToken)
			{
				foundToken = false;

				// Check if there is a "with" argument
				if (tokens.PeekIgnore(new string[]{" ","\t","\n"}) == "with")
				{
					tokens.Skip(new string[]{" ","\t","\n"});
					tokens.Next();
					tokens.Skip(new string[]{" ","\t"});
					tokens.Next(); // TODO: Don't ignore the with argument
					foundToken = true;
				}
				// Check if there is an "at" argument
				else if (tokens.PeekIgnore(new string[]{" ","\t","\n"}) == "at")
				{
					tokens.Skip(new string[]{" ","\t","\n"});
					tokens.Next();
					tokens.Skip(new string[]{" ","\t"});
					string alignment = tokens.Next();
					m_alignment = Util.RenPyAlignmentConverter.FromString(alignment);
					foundToken = true;
				}
				// TODO: Check for other arguments
			}
		}

		public override void Execute(RenPyState state)
		{
			string filename = state.GetImageFilename(m_imageName);
			Texture2D tex = state.Script.GetImage(filename);
			var image = new RenPyImageData(ref tex, m_alignment);

			state.Visual.AddImage(m_imageName, ref image);
		}

		public override string ToDebugString()
		{
			string str = "show";
			str += " \"" + m_imageName + "\"";
			return str;
		}
	}
}
