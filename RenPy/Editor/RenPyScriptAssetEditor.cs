using UnityEditor;
using UnityEngine;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Displays RenPyScriptAssets in the inspector.
	/// </summary>
	[CustomEditor(typeof(RenPyScriptAsset))]
	public class RenPyScriptAssetEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			var script = target as RenPyScriptAsset;
			if (null == script) {
				return;
			}

			// If the asset is empty, display a help box
			if (string.IsNullOrEmpty(script.Source)) {
				string msg = "Script is empty!";
				EditorGUILayout.HelpBox(msg, MessageType.Error);
			}

			// Otherwise, display the asset's contents
			else {
				GUILayout.Label(script.Source);
			}
		}
	}
}
