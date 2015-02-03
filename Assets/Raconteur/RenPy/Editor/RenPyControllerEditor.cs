#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

using DPek.Raconteur.RenPy.Display;

namespace DPek.Raconteur.RenPy.Editor
{
	/// <summary>
	/// Draws a custom inspector for RenPyDisplayState scripts.
	/// </summary>
	[CustomEditor(typeof(RenPyController), true)]
	public class RenPyControllerEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			RenPyController display = (RenPyController)target;

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
