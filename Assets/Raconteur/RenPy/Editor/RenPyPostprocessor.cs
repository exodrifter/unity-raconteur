#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;

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
			script.Blocks = Parser.RenPyParser.Parse(handle.lines);

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
			} else {
				AssetDatabase.CreateAsset(script, handle.path);
			}
			
			// Serialize the parsed script
			asset = AssetDatabase.LoadMainAssetAtPath(handle.path);
			SerializeChildren(script.Blocks, ref asset);

			AssetDatabase.SaveAssets();
		}

		private static void SerializeChildren(List<RenPyBlock> blocks,
		                                      ref Object asset)
		{
			if(blocks == null) {
				return;
			}

			var flags = HideFlags.HideInHierarchy;
			foreach (var block in blocks) {
				foreach(var statement in block.Statements) {
					statement.hideFlags = flags;
					AssetDatabase.AddObjectToAsset(statement, asset);
					if(statement is RenPyVariable) {
						var v = statement as RenPyVariable;
						v.Operator.hideFlags = flags;
						AssetDatabase.AddObjectToAsset(v.Operator, asset);
					} else if(statement is RenPyIf) {
						var v = statement as RenPyIf;
						v.Operator.hideFlags = flags;
						AssetDatabase.AddObjectToAsset(v.Operator, asset);
					}
					SerializeChildren(statement.NestedBlocks, ref asset);
				}
				block.hideFlags = flags;
				AssetDatabase.AddObjectToAsset(block, asset);
			}
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
