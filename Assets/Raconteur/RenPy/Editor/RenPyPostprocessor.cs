#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Processes RenPy files.
	/// </summary>
	public class RenPyPostprocessor : AssetPostprocessor
	{
		/// <summary>
		/// Finds script.rpy files and saves data about them to custom asset
		/// files.
		/// </summary>
		private static void OnPostprocessAllAssets(string[] importedAssets,
		        string[] deletedAssets,
		        string[] movedAssets,
		        string[] movedFromPath)
		{
			foreach (string assetPath in importedAssets) {
				CreateRenPyAsset(GetRenPyFileHandle(assetPath));
			}

			foreach (string assetPath in deletedAssets) {
				RemoveRenPyAsset(GetRenPyFileHandle(assetPath, false));
			}

			foreach (string assetPath in movedAssets) {
				CreateRenPyAsset(GetRenPyFileHandle(assetPath));
			}

			foreach (string assetPath in movedFromPath) {
				RemoveRenPyAsset(GetRenPyFileHandle(assetPath, false));
			}
		}

		/// <summary>
		/// Creates a RenPyFile if the passed asset path is a Ren'Py script.
		/// </summary>
		/// <param name="assetPath">
		/// The path of the asset to create a RenPyFile for.
		/// </param>
		/// <param name="getContents">
		/// Whether or not to read the contents of the Ren'Py script.
		/// </param>
		/// <returns>
		/// The RenPyFile for the passed asset path.
		/// </returns>
		private static RenPyFile GetRenPyFileHandle(string assetPath,
		        bool getContents = true)
		{
			// Check if the file is a Ren'Py script
			string filename = Path.GetFileNameWithoutExtension(assetPath);
			string filetype = Path.GetExtension(assetPath);
			if (filetype != ".rpy" || filename != "script") {
				return null;
			}

			// Get the folder name
			string foldername = Path.GetDirectoryName(assetPath);  // "game"
			foldername = Path.GetDirectoryName(foldername);  // folder path
			foldername = Path.GetFileName(foldername);

			// Get the asset's path
			string folderPath = Path.GetDirectoryName(assetPath);
			string path = folderPath + Path.DirectorySeparatorChar;
			path += "script-" + foldername.ToLower() + ".asset";

			// Get the file's contents
			List<string> content = new List<string>();
			if (getContents) {
				StreamReader scanner = new StreamReader(assetPath);
				while (scanner.Peek() > 0) {
					string line = scanner.ReadLine();
					content.Add(line);
				}
			}
			string[] lines = content.Count != 0 ? content.ToArray() : null;

			return new RenPyFile(foldername, path, folderPath, lines);
		}

		/// <summary>
		/// Creates a custom asset file with the passed RenPyFile.
		/// </summary>
		/// <param name="handle">
		/// The RenPyFile asset to create
		/// </param>
		private static void CreateRenPyAsset(RenPyFile handle)
		{
			if (handle == null) {
				return;
			}

			RenPyScriptAsset script;
			script = ScriptableObject.CreateInstance<RenPyScriptAsset>();
			script.name = Path.GetFileNameWithoutExtension(handle.path);
			script.Title = handle.name;
			script.Lines = handle.lines;

			// Construct the system path of the Ren'Py asset folder
			string dataPath  = Application.dataPath;
			string folderPath = dataPath.Substring(0, dataPath.Length - 6);
			folderPath += handle.folder;

			// Get system file paths of all files in the Ren'Py asset folder
			string[] filePaths = Directory.GetFiles(folderPath);

			// Find supported assets for the RenPyScriptAsset
			List<string> audioKeys = new List<string>();
			List<AudioClip> audioValues = new List<AudioClip>();
			foreach (string filePath in filePaths) {
				string assetPath = filePath.Substring(dataPath.Length - 6);
				string filename = Path.GetFileName(filePath);

				System.Type objType = typeof(Object);
				Object obj = AssetDatabase.LoadAssetAtPath(assetPath, objType);
				if (obj == null) {
					continue;
				}

				if (obj.GetType() == typeof(AudioClip)) {
					audioKeys.Add(filename);
					audioValues.Add(obj as AudioClip);
				}

				// TODO: Add support for images
			}

			// Save assets to the RenPyScriptAsset
			script.audioKeys = audioKeys.ToArray();
			script.audioValues = audioValues.ToArray();

			// Create or update the RenPyScriptAsset
			Object asset = AssetDatabase.LoadMainAssetAtPath(handle.path);
			RenPyScriptAsset outputScript = asset as RenPyScriptAsset;
			if (outputScript != null) {
				EditorUtility.CopySerialized(script, outputScript);
			} else {
				AssetDatabase.CreateAsset(script, handle.path);
			}
			AssetDatabase.SaveAssets();
		}

		/// <summary>
		/// Removes the custom asset file with the passed RenPyFile.
		/// </summary>
		/// <param name="handle">
		/// The RenPyFile asset to delete.
		/// </param>
		private static void RemoveRenPyAsset(RenPyFile handle)
		{
			if (handle != null) {
				AssetDatabase.DeleteAsset(handle.path);
			}
		}
	}
}

#endif
