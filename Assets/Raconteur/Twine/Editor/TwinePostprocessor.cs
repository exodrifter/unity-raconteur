#if UNITY_EDITOR

using DPek.Raconteur.Twine.Parser;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DPek.Raconteur.Twine.Editor
{
	/// <summary>
	/// Processes and parses Twine .twee files during the asset post process
	/// phase.
	/// </summary>
	public class TwinePostprocessor : AssetPostprocessor
	{
		/// <summary>
		/// Finds *.twee files and saves data about them to custom asset files.
		/// </summary>
		private static void OnPostprocessAllAssets(string[] importedAssets,
		                                           string[] deletedAssets,
		                                           string[] movedAssets,
		                                           string[] movedFromPath)
		{
			foreach (string assetPath in importedAssets) {
                CreateAsset(GetTwineFileHandle(assetPath, true));
			}

			foreach (string assetPath in deletedAssets) {
				RemoveAsset(GetTwineFileHandle(assetPath, false));
			}

			foreach (string assetPath in movedAssets) {
				CreateAsset(GetTwineFileHandle(assetPath, true));
			}

			foreach (string assetPath in movedFromPath) {
				RemoveAsset(GetTwineFileHandle(assetPath, false));
			}
		}

		/// <summary>
		/// Creates a TwineFileHandle if the passed asset path is a Twine
		/// script.
		/// </summary>
		/// <param name="assetPath">
		/// The path of the asset to create a TwineFileHandle for.
		/// </param>
		/// <param name="getContents">
		/// Whether or not to read the contents of the Twine script.
		/// </param>
		/// <returns>
		/// The TwineFileHandle for the passed asset path.
		/// </returns>
		private static TwineFileHandle GetTwineFileHandle(string assetPath,
		                                                  bool getContents)
		{
			// Check if the file is a Twine script
			string filename = Path.GetFileNameWithoutExtension(assetPath);
			string filetype = Path.GetExtension(assetPath);
			if (filetype != ".twee" && filetype != ".tw")
			{
				return null;
			}

			// Get the asset's path
			string folderPath = Path.GetDirectoryName(assetPath);
			string path = folderPath + Path.DirectorySeparatorChar;
			path += "script-" + filename.ToLower() + ".asset";

			// Get the file's contents
			string content = null;
			if (getContents) {
				StreamReader scanner = new StreamReader(assetPath);
				content = scanner.ReadToEnd();
			}

			return new TwineFileHandle(path, content);
		}

		/// <summary>
		/// Creates a custom asset file.
		/// </summary>
		/// <param name="handle">
		/// The TwineFileHandle asset to create.
		/// </param>
		private static void CreateAsset(TwineFileHandle handle)
		{
			if (handle == null) {
				return;
			}

			// Create the asset
			var script = ScriptableObject.CreateInstance<TwineScriptAsset>();
			script.content = handle.Content;

			// Update the asset on disk
			Object asset = AssetDatabase.LoadMainAssetAtPath(handle.AssetPath);
			TwineScriptAsset outputScript = asset as TwineScriptAsset;
			if (outputScript != null) {
				EditorUtility.CopySerialized(script, outputScript);
				ScriptableObject.DestroyImmediate(script, true);
			} else {
				AssetDatabase.CreateAsset(script, handle.AssetPath);
			}
			AssetDatabase.SaveAssets();
		}

		/// <summary>
		/// Removes the custom asset file.
		/// </summary>
		/// <param name="handle">
		/// The TwineFileHandle asset to delete.
		/// </param>
		private static void RemoveAsset(TwineFileHandle handle)
		{
			if (handle != null) {
				AssetDatabase.DeleteAsset(handle.AssetPath);
			}
		}
	}
}

#endif
