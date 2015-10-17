using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Processes and parses Ren'Py .rpy files during the asset post process
	/// phase.
	/// 
	/// This class is required, because trying to access files with Unity's
	/// TextAsset class will require the files to have an extension that is
	/// not .rpy. To get around this, this class accesses the file in the
	/// post process phase and saves it as a custom unity asset for later
	/// access.
	/// </summary>
	public class RenPyPostprocessor : AssetPostprocessor
	{
		/// <summary>
		/// Finds script.rpy files and saves data about them to custom asset
		/// files.
		/// </summary>
		private static void OnPostprocessAllAssets(string[] imported,
			string[] deleted, string[] moved, string[] movedFrom)
		{
			foreach (string path in imported) {
				CreateRenPyAsset(GetHandle(path));
			}

			foreach (string path in deleted) {
				RemoveRenPyAsset(GetHandle(path));
			}

			for (int i = 0; i < moved.Length; ++i) {
				string from = movedFrom[i];
				string to = moved[i];
				RenameRenPyAsset(GetHandle(from), GetHandle(to));
			}
		}

		/// <summary>
		/// Creates a RenPyFile if the passed asset path is a Ren'Py script.
		/// </summary>
		/// <param name="assetPath">
		/// The path of the asset to create a RenPyFile for.
		/// </param>
		/// <returns>
		/// The RenPyFile for the passed asset path.
		/// </returns>
		private static RenPyFile GetHandle(string assetPath)
		{
			// Check if the file is a Ren'Py script
			string filename = Path.GetFileNameWithoutExtension(assetPath);
			string filetype = Path.GetExtension(assetPath);
			if (filetype != ".rpy" || filename != "script") {
				return null;
			}

			// Get the folder path
			string folder = Path.GetDirectoryName(assetPath);

			// Get the folder name
			string name = Path.GetFileName(Path.GetDirectoryName(folder));

			return new RenPyFile(name, folder);
		}

		/// <summary>
		/// Creates a custom asset file with the passed handle.
		/// </summary>
		/// <param name="handle">
		/// The handle of the asset to create.
		/// </param>
		private static void CreateRenPyAsset(RenPyFile handle)
		{
			if (handle == null) {
				return;
			}

			var script = CreateScript(handle);

			// Create or update the RenPyScriptAsset
			var asset = AssetDatabase.LoadMainAssetAtPath(handle.AssetPath);
			var outputScript = asset as RenPyScriptAsset;
			if (outputScript != null) {
				EditorUtility.CopySerialized(script, outputScript);
				Object.DestroyImmediate(script, true);
			}
			else {
				AssetDatabase.CreateAsset(script, handle.AssetPath);
			}
			AssetDatabase.SaveAssets();
		}

		/// <summary>
		/// Removes the custom asset file with the passed RenPyFile.
		/// </summary>
		/// <param name="handle">
		/// The handle of the asset to delete.
		/// </param>
		private static void RemoveRenPyAsset(RenPyFile handle)
		{
			if (handle != null) {
				AssetDatabase.DeleteAsset(handle.AssetPath);
			}
		}

		/// <summary>
		/// Updates the custom asset file that has been moved from one path
		/// to another.
		/// </summary>
		/// <param name="from">
		/// The handle of the asset's old path.
		/// </param>
		/// <param name="to">
		/// The handle of the asset to update.
		/// </param>
		private static void RenameRenPyAsset(RenPyFile from, RenPyFile to)
		{
			if (null == from || null == to) {
				return;
			}

			var script = CreateScript(to);

			// Modify the old path to match the new directory
			var oldPath = to.folder + System.IO.Path.DirectorySeparatorChar
				+ "script-" + from.name.ToLower() + ".asset";

			var asset = AssetDatabase.LoadMainAssetAtPath(oldPath);
			var outputScript = asset as RenPyScriptAsset;
			if (outputScript != null) {
				EditorUtility.CopySerialized(script, outputScript);
				AssetDatabase.RenameAsset(oldPath, "script-" + to.name.ToLower());
				Object.DestroyImmediate(script, true);
			}
			else {
				AssetDatabase.CreateAsset(script, to.AssetPath);
			}
			AssetDatabase.SaveAssets();
		}

		private static RenPyScriptAsset CreateScript(RenPyFile handle)
		{
			var script = ScriptableObject.CreateInstance<RenPyScriptAsset>();
			script.name = Path.GetFileNameWithoutExtension(handle.AssetPath);
			script.Title = handle.name;
			using (var scanner = new StreamReader(handle.ScriptPath)) {
				script.Source = scanner.ReadToEnd().Replace("\r\n", "\n");
			}
			script.audio = new Dictionary<string, AudioClip>();
			script.image = new Dictionary<string, Texture2D>();

			// Construct the system path of the Ren'Py asset folder
			string dataPath = Application.dataPath;
			string folderPath = dataPath.Substring(0, dataPath.Length - 6);
			folderPath += handle.folder;

			// Find supported assets for the RenPyScriptAsset
			foreach (string path in Directory.GetFiles(folderPath)) {
				string assetPath = path.Substring(dataPath.Length - 6);
				string filename = Path.GetFileName(path);

				var obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
				if (obj == null) {
					continue;
				}

				if (obj.GetType() == typeof(AudioClip)) {
					script.audio.Add(filename, obj as AudioClip);
				}

				if (obj.GetType() == typeof(Texture2D)) {
					script.image.Add(filename, obj as Texture2D);
				}
			}

			return script;
		}
	}
}
