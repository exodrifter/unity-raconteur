#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Displays .asset RenPyScriptAssets in the inspector.
	/// </summary>
	[CustomEditor(typeof(RenPyScriptAsset))]
	public class RenPyScriptAssetEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			RenPyScriptAsset script = target as RenPyScriptAsset;
			if (null == script) {
				return;
			}

			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.white;

			// If the asset is empty, display a help box
			if (script.Lines == null || script.Lines.Length == 0) {
				EditorGUILayout.HelpBox("Script is empty!", MessageType.Error);
			}

			// Otherwise, display the asset's contents
			else {
				string str = "";
				for (int i = 0; i < script.Lines.Length; i++) {
					str += script.Lines[i] + "\n";
				}

				GUILayout.Label(str, style);
			}
		}
	}
}

#endif
