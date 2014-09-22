#if UNITY_EDITOR

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Class that stores information about a Ren'Py script.
	/// </summary>
	public class RenPyFile
	{
		/// <summary>
		/// The name of the Ren'Py script.
		/// </summary>
		public readonly string name;

		/// <summary>
		/// The path to the Ren'Py script.
		/// </summary>
		public readonly string path;

		/// <summary>
		/// The path to the folder containing the Ren'Py script.
		/// </summary>
		public readonly string folder;

		/// <summary>
		/// The content of the Ren'Py script.
		/// </summary>
		public readonly string[] lines;

		/// <summary>
		/// Creates a new RenPyFileHandle with the passed information.
		/// </summary>
		/// <param name="name">
		/// The name of the Ren'Py script.
		/// </param>
		/// <param name="path">
		/// The path to the Ren'Py script.
		/// </param>
		/// <param name="folder">
		/// The path to the folder containing the Ren'Py script.
		/// </param>
		/// <param name="lines">
		/// The content of the Ren'Py script.
		/// </param>
		public RenPyFile(string name, string path, string folder, string[] lines)
		{
			this.name = name;
			this.path = path;
			this.folder = folder;
			this.lines = lines;
		}
	}
}

#endif
