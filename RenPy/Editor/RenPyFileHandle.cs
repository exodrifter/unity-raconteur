using System.IO;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Class that stores information about a Ren'Py script file.
	/// </summary>
	public class RenPyFile
	{
		/// <summary>
		/// The name of the Ren'Py script.
		/// </summary>
		public readonly string name;

		/// <summary>
		/// The path to the folder containing the Ren'Py script.
		/// </summary>
		public readonly string folder;

		/// <summary>
		/// The path to the Raconteur asset.
		/// </summary>
		public string AssetPath
		{
			get
			{
				return folder + System.IO.Path.DirectorySeparatorChar +
					"script-" + name.ToLower() + ".asset";
			}
		}

		/// <summary>
		/// The path to the .rpy script.
		/// </summary>
		public string ScriptPath
		{
			get
			{
				return folder + System.IO.Path.DirectorySeparatorChar +
					"script.rpy";
			}
		}

		/// <summary>
		/// Creates a new RenPyFileHandle with the passed information.
		/// </summary>
		/// <param name="name">
		/// The name of the Ren'Py script.
		/// </param>
		/// <param name="folder">
		/// The path to the folder containing the Ren'Py script.
		/// </param>
		public RenPyFile(string name, string folder)
		{
			this.name = name;
			this.folder = folder;
		}
	}
}
