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
		private readonly string m_assetPath;
		public string AssetPath
		{
			get { return m_assetPath; }
		}

		/// <summary>
		/// The content of the Ren'Py script.
		/// </summary>
		private readonly string m_content;
		public string Content
		{
			get { return m_content; }
		}

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
		/// <param name="content">
		/// The content of the Twine script.
		/// </param>
		public TwineFileHandle(string assetPath, string content)
		{
			this.m_assetPath = assetPath;
			this.m_content = content;
		}
	}
}

#endif
