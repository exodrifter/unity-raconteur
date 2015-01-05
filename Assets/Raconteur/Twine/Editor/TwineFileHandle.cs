#if UNITY_EDITOR

namespace DPek.Raconteur.Twine.Editor
{
	/// <summary>
	/// Stores information about a Twine script file.
	/// </summary>
	public class TwineFileHandle
	{
		/// <summary>
		/// The path to the location of the Twine script asset file.
		/// </summary>
		public readonly string assetPath;

		/// <summary>
		/// The content of the Ren'Py script.
		/// </summary>
		public readonly string[] lines;

		/// <summary>
		/// Creates a new TwineFileHandle with the passed information.
		/// </summary>
		/// <param name="name">
		/// The name of the Twine script.
		/// </param>
		/// <param name="path">
        /// The path to the Twine script.
		/// </param>
		/// <param name="folder">
        /// The path to the folder containing the Twine script.
		/// </param>
		/// <param name="lines">
        /// The content of the Twine script.
		/// </param>
		public TwineFileHandle(string assetPath, string[] lines)
		{
            this.assetPath = assetPath;
			this.lines = lines;
		}
	}
}

#endif
