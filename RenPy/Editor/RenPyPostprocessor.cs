﻿#if UNITY_EDITOR

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Processes and parses Ren'Py .rpy files during the asset post process
	/// phase.
	/// 
	/// Besides parsing .rpy files at import time, this class is also required
	/// to access the .rpy files in the first place. This is because trying to
	/// access the files with Unity's TextAsset class will require the files to
	/// have an extension that is not .rpy. To get around this, this class
	/// accesses the file in the post process phase and saves it as a custom
	/// unity asset for later access.
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
				CreateRenPyAsset(GetRenPyFileHandle(assetPath, true));
			}

			foreach (string assetPath in deletedAssets) {
				RemoveRenPyAsset(GetRenPyFileHandle(assetPath, false));
			}

			foreach (string assetPath in movedAssets) {
				CreateRenPyAsset(GetRenPyFileHandle(assetPath, true));
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
		                                            bool getContents)
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
			var content = new List<string>();
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
		/// The RenPyFile asset to create.
		/// </param>
		private static void CreateRenPyAsset(RenPyFile handle)
		{
			if (handle == null) {
				return;
			}

			var script = ScriptableObject.CreateInstance<RenPyScriptAsset>();
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
			List<string> imageKeys = new List<string>();
			List<Texture2D> imageValues = new List<Texture2D>();
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

				if (obj.GetType() == typeof(Texture2D)) {
					imageKeys.Add(filename);
					imageValues.Add(obj as Texture2D);
				}
			}

			// Save assets to the RenPyScriptAsset
			script.audioKeys = audioKeys.ToArray();
			script.audioValues = audioValues.ToArray();
			script.imageKeys = imageKeys.ToArray();
			script.imageValues = imageValues.ToArray();

			// Create or update the RenPyScriptAsset
			Object asset = AssetDatabase.LoadMainAssetAtPath(handle.path);
			RenPyScriptAsset outputScript = asset as RenPyScriptAsset;
			if (outputScript != null) {
				EditorUtility.CopySerialized(script, outputScript);
				ScriptableObject.DestroyImmediate(script, true);
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
