#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;

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
			style.normal.textColor = Color.gray;
			style.margin = new RectOffset(0,0,0,0);

			// If the asset is empty, display a help box
			if (script.Lines == null || script.Lines.Length == 0) {
				EditorGUILayout.HelpBox("Script is empty!", MessageType.Error);
			}

			// Otherwise, display the asset's contents
			else {
				GUILayout.Label(script.name, style);
				for (int i = 0; i < script.Lines.Length; i++) {
					GUILayout.Label(script.Lines[i], style);
				}
			}
		}
	}
}

#endif
