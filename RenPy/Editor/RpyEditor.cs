using UnityEditor;
using UnityEngine;
using System.IO;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Displays .rpy files in the inspector.
	/// </summary>
	[CustomEditor(typeof(DefaultAsset))]
	public class RpyEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			if (null == target) {
				return;
			}

			// Check if this object is a .rpy file
			string rpyPath = AssetDatabase.GetAssetPath(target);
			string ext = Path.GetExtension(rpyPath);
			if (".rpy" != ext) {
				return;
			}

			// Load the file's contents
			string content = "";
			using (var scanner = new StreamReader(rpyPath)) {
				content = scanner.ReadToEnd();
			}

			// If the file is empty, display an info help box
			if (string.IsNullOrEmpty(content)) {
				string msg = "Script is empty!";
				EditorGUILayout.HelpBox(msg, MessageType.Info);
			}

			// Otherwise, display the file's contents
			else {
				GUILayout.Label(content);
			}
		}
	}
}
