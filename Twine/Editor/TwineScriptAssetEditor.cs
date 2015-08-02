#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

using DPek.Raconteur.Twine.Parser;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Displays .asset TwineScriptAssets in the inspector.
	/// </summary>
	[CustomEditor(typeof(TwineScriptAsset))]
	public class TwineScriptAssetEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			TwineScriptAsset script = target as TwineScriptAsset;
			if (null == script) {
				return;
			}

			// If the asset is empty, display a help box
			if (script.content == null || script.content.Length == 0) {
				EditorGUILayout.HelpBox("Script is empty!", MessageType.Error);
			}

			// Otherwise, display the asset's contents
			else {
				GUILayout.Label(script.content);
			}
		}
	}
}

#endif
