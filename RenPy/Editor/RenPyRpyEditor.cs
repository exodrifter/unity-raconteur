#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Displays .rpy files in the inspector.
	/// </summary>
	[CustomEditor(typeof(Object))]
	public class RenPyRpyEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			if (null == target) {
				return;
			}

			// Check if this object is a .rpy file
			string rpyPath = AssetDatabase.GetAssetPath(target);
			string ext = Path.GetExtension(rpyPath);
			if (ext == ".rpy") {

				// Load the file's contents
				StreamReader scanner = new StreamReader(rpyPath);
				string content = "";
				while (scanner.Peek() > 0) {
					content += scanner.ReadLine() + "\n";
				}

				// If the file is empty, display an info help box
				if (content == string.Empty) {
					string msg = "Script is empty!";
					EditorGUILayout.HelpBox(msg, MessageType.Info);
				}

				// Otherwise, display the file's contents
				else {
					GUIStyle style = new GUIStyle();
					style.normal.textColor = Color.white;
					GUILayout.Label(content, style);
				}
			}
		}
	}
}

#endif
