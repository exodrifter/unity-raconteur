#if UNITY_EDITOR

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

		/// <summary>
		/// Serializes the RenPyBlocks and all of its children into an asset.
		/// </summary>
		/// <param name="blocks">
		/// The blocks to save.
		/// </param>
		/// <param name="asset">
		/// The asset to save the blocks in.
		/// </param>
		private static void SerializeChildren(List<RenPyBlock> blocks,
		                                      ref Object asset)
		{
			if(blocks == null) {
				return;
			}

			// Save each block
			foreach (var block in blocks) {
				block.hideFlags = HideFlags.HideInHierarchy;
				AssetDatabase.AddObjectToAsset(block, asset);

				// Save each statement in each block
				foreach(var statement in block.Statements) {
					statement.hideFlags = HideFlags.HideInHierarchy;
					AssetDatabase.AddObjectToAsset(statement, asset);

					// Save each statement's children
					SaveScriptableObjects(statement, ref asset);
					SerializeChildren(statement.NestedBlocks, ref asset);
				}
			}
		}

		/// <summary>
		/// Saves every public and non-public member of type ScriptableObject
		/// in the object into the asset. If a ScriptableObject is found, its
		/// members will also be checked for ScriptableObjects to save.
		/// 
		/// Note that this method will not save member variables that are not
		/// ScriptableObjects but contain ScriptableObjects within them. For
		/// example, a member variable of type List<ScriptableObject> will not
		/// be saved.
		/// </summary>
		/// <param name="obj">
		/// The object that should be checked for ScriptableObjects.
		/// </param>
		/// <param name="asset">
		/// The asset to save the ScriptableObjects to.
		/// </param>
		private static void SaveScriptableObjects(object obj, ref Object asset)
		{
			if (obj == null) {
				return;
			}

			var scriptableType = typeof(ScriptableObject);
			var bindFlags = BindingFlags.FlattenHierarchy | BindingFlags.Public
						  | BindingFlags.NonPublic | BindingFlags.Instance;
			FieldInfo[] fields = obj.GetType().GetFields(bindFlags);

			// Search the fields for a scriptable object
			foreach (FieldInfo field in fields) {
				if (scriptableType.IsAssignableFrom(field.FieldType)) {
					Object child = field.GetValue(obj) as Object;
					if (child == null) {
						continue;
					}

					// Save the scriptable object
					child.hideFlags = HideFlags.HideInHierarchy;
					if (!AssetDatabase.Contains(child)) {
						AssetDatabase.AddObjectToAsset(child, asset);
					}

					// Save this scriptable object's children
					SaveScriptableObjects(child, ref asset);
				}
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
