#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Draws a custom inspector for RenPyDisplayState scripts.
	/// </summary>
	[CustomEditor(typeof(RenPyDisplayState), true)]
	public class RenPyDisplayStateEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			RenPyDisplayState display = (RenPyDisplayState) target;

			GUILayout.Space(5);
			GUILayout.Label("Debug Controls:");

			GUI.enabled = UnityEngine.Application.isPlaying;
			if (GUILayout.Button("Start Dialog")) {
				display.StartDialog();
			}
			if (GUILayout.Button("Stop Dialog")) {
				display.StopDialog();
			}
		}
	}
}

#endif
