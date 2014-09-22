#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;

namespace DPek.Raconteur
{
	/// <summary>
	/// Static Raconteur data.
	/// </summary>
#if UNITY_EDITOR
	[InitializeOnLoad]
	public class Static : EditorWindow
#else
	public class Static
#endif
	{
		#region Static Debug Variables

#if UNITY_EDITOR

		private static bool m_debug;
		private static bool m_skipDialog;
		private static bool m_debugLines;

		/// <summary>
		/// Whether or not to show debugging information
		/// </summary>
		public static bool DebugMode
		{
			get {
				return m_debug;
			}
		}

		/// <summary>
		/// Whether or not to skip the dialog and go straight to the choices
		/// </summary>
		public static bool SkipDialog
		{
			get {
				return m_debug && m_skipDialog;
			}
		}

		/// <summary>
		/// Whether or not to print debugging information for each line
		/// </summary>
		private static bool DebugLines
		{
			get {
				return m_debug && m_debugLines;
			}
		}

#else

		public static bool DebugMode
		{
			get {
				return false;
			}
		}

		public static bool SkipDialog
		{
			get {
				return false;
			}
		}

		public static bool DebugLines
		{
			get {
				return false;
			}
		}

#endif

		#endregion

		#region Static Variables

		private static Dictionary<string, string> m_variables;

		public static Dictionary<string, string> Vars
		{
			get {
				m_variables = m_variables ?? new Dictionary<string, string>();
				return m_variables;
			}
		}

		#endregion

		#region Unity UI

#if UNITY_EDITOR

		[MenuItem("Window/Raconteur")]
		public static void ShowWindow()
		{
			EditorWindow window = EditorWindow.GetWindow(typeof(Static));
			window.title = "Raconteur";
		}

		void OnGUI()
		{
			bool oldDebug = m_debug;
			bool oldSkip = m_skipDialog;
			bool oldLines = m_debugLines;

			m_debug = EditorGUILayout.BeginToggleGroup("Enable Debugging", m_debug);
			m_skipDialog = EditorGUILayout.Toggle("Skip Dialog", m_skipDialog);
			m_debugLines = EditorGUILayout.Toggle("Debug Lines", m_debugLines);
			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Variables");
			if (!Application.isPlaying) {
				string msg = "Can only show variables in play mode.";
				EditorGUILayout.HelpBox(msg, MessageType.Info, true);
			} else if (Vars.Keys.Count == 0) {
				string msg = "No variables.";
				EditorGUILayout.HelpBox(msg, MessageType.Info, true);
			} else {
				// TODO: Allow editing of variables
				GUI.enabled = false;
				foreach (KeyValuePair<string, string> kvp in Vars) {
					EditorGUILayout.TextField(kvp.Key, kvp.Value);
				}
				GUI.enabled = true;
			}

			// Save if any options have changed
			if (oldDebug != m_debug || oldSkip != m_skipDialog
			    || oldLines != m_debugLines) {
				SaveEditorPrefs();
			}

			// Update the GUI
			this.Repaint();
		}

#endif

		#endregion

		#region Unity Editor Prefs

#if UNITY_EDITOR

		/// <summary>
		/// Load this window's preferences when Unity is loaded.
		/// </summary>
		static Static()
		{
			LoadEditorPrefs();
		}

		/// <summary>
		/// Load this window's preferences when this window gains focus.
		/// </summary>
		void OnFocus()
		{
			LoadEditorPrefs();
		}

		/// <summary>
		/// Loads this window's preferences by querying EditorPrefs.
		/// </summary>
		static void LoadEditorPrefs()
		{
			m_debug = EditorPrefs.GetBool("raconteur-m_debug", false);
			m_skipDialog = EditorPrefs.GetBool("raconteur-m_skipDialog", false);
			m_debugLines = EditorPrefs.GetBool("raconteur-m_debugLines", false);
		}

		/// <summary>
		/// Saves this window's preferences by using EditorPrefs.
		/// </summary>
		static void SaveEditorPrefs()
		{
			if (m_debug) {
				EditorPrefs.SetBool("raconteur-m_debug", m_debug);
			} else {
				EditorPrefs.DeleteKey("raconteur-m_debug");
			}
			if (m_skipDialog) {
				EditorPrefs.SetBool("raconteur-m_skipDialog", m_skipDialog);
			} else {
				EditorPrefs.DeleteKey("raconteur-m_skipDialog");
			}
			if (m_debugLines) {
				EditorPrefs.SetBool("raconteur-m_debugLines", m_debugLines);
			} else {
				EditorPrefs.DeleteKey("raconteur-m_debugLines");
			}
		}

#endif

		#endregion

		public static void Log(string str)
		{
#if UNITY_EDITOR
			if (DebugLines) {
				Debug.Log(str);
			}
#endif
		}
	}
}
