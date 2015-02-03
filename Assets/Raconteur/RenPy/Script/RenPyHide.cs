using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

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

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyHide(ref Scanner tokens) : base(RenPyStatementType.HIDE)
		{
			tokens.Seek("hide");
			tokens.Next();

			string[] arr = new string[] { "\n", "with" };
			m_imageName = tokens.Seek(arr).Trim();
			tokens.Skip(new string[]{" ","\t","\n"});

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

				// TODO: Check for other arguments
			}
		}

		public override void Execute(RenPyState state)
		{
			state.Visual.RemoveImage(m_imageName);
		}

		public override string ToDebugString()
		{
			string str = "hide";
			str += " \"" + m_imageName + "\"";
			return str;
		}
	}
}
