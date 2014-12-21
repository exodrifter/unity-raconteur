using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
namespace DPek.Raconteur
{
	/// <summary>
	/// Static Raconteur data.
	/// </summary>
	[InitializeOnLoad]
	public class Static : EditorWindow
	{
		#region Static Debug Variables
		
		private static bool m_debug;
		private static bool m_skipDialog;
		private static bool m_debugLines;
		private static bool m_muteAudio;
		
		/// <summary>
		/// Whether or not to show debugging information.
		/// </summary>
		public static bool DebugMode
		{
			get {
				return m_debug;
			}
		}
		
		/// <summary>
		/// Whether or not to skip the dialog and go straight to the choices.
		/// </summary>
		public static bool SkipDialog
		{
			get {
				return m_debug && m_skipDialog;
			}
		}
		
		/// <summary>
		/// Whether or not to print debugging information for each line.
		/// </summary>
		private static bool DebugLines
		{
			get {
				return m_debug && m_debugLines;
			}
		}
		
		/// <summary>
		/// Whether or not to mute audio.
		/// </summary>
		public static bool MuteAudio
		{
			get {
				return m_debug && m_muteAudio;
			}
		}
		
		#endregion
		
		#region Static Variables
		
		/// <summary>
		/// The variables in the game.
		/// </summary>
		private static Dictionary<string, string> m_variables;
		public static Dictionary<string, string> Vars
		{
			get {
				m_variables = m_variables ?? new Dictionary<string, string>();
				return m_variables;
			}
		}
		private static bool new_variable_time = false;
		private static bool show_add_variable = true;
		
		private static string var_name = "";
		private static string var_value = "";
		
		private static string atex = "Assets/Raconteur/add.png";
		private static Texture addtext = Resources.LoadAssetAtPath(atex, typeof(Texture)) as Texture;
		
		private static string dtex = "Assets/Raconteur/delete.png";
		private static Texture deltext = Resources.LoadAssetAtPath(dtex, typeof(Texture)) as Texture;
		
		private static GUIContent content = new GUIContent();
		private static GUIStyle style = new GUIStyle("Assets/Raconteur/borderless");
		
		private static int juggle = 0;
		private static int space = 18;
		#endregion
		
		#region Unity UI
		
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
			bool oldMuteAudio = m_muteAudio;
			
			m_debug = EditorGUILayout.BeginToggleGroup("Enable Debugging", m_debug);
			m_skipDialog = EditorGUILayout.Toggle("Skip Dialog", m_skipDialog);
			m_debugLines = EditorGUILayout.Toggle("Debug Lines", m_debugLines);
			m_muteAudio = EditorGUILayout.Toggle("Mute Audio", m_muteAudio);
			EditorGUILayout.EndToggleGroup();
			
			EditorGUILayout.Space();
			
			var boldtext = new GUIStyle (GUI.skin.label);
			boldtext.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("Variables", boldtext);
			
			if (!Application.isPlaying) {
				string msg = "Can only show variables in play mode.";
				EditorGUILayout.HelpBox(msg, MessageType.Info, true);
			} else if (Vars.Keys.Count == 0) {
				string msg = "No variables.";
				EditorGUILayout.HelpBox(msg, MessageType.Info, true);
			} else {
				var keys = new List<string>(Vars.Keys);
				foreach (string key in keys) {
					GUILayout.BeginHorizontal();
					var temp = EditorGUILayout.TextField(key, Vars[key], GUILayout.Width(225));
					Vars[key] = temp;
					if (GUILayout.Button (deltext, style, GUILayout.Width(space), GUILayout.Height(space))) {
						juggle -= space;
						Vars.Remove(key);
					}
					GUILayout.EndHorizontal();
				}
			}
			
			// Save if any options have changed
			if (oldDebug != m_debug || oldSkip != m_skipDialog
			    || oldLines != m_debugLines || oldMuteAudio != m_muteAudio) {
				SaveEditorPrefs();
			}
			
			// Begin add Button Code
			content.image = addtext;
			content.text = " Add Variable";
			content.tooltip = "oh hai!";
			
			if (show_add_variable && GUILayout.Button(content, GUILayout.Width(100), GUILayout.Height(30))) {
				new_variable_time = !new_variable_time;
				show_add_variable = !show_add_variable;
			}
			
			if (new_variable_time) {
				GUI.Box(new Rect(0, 117 + juggle, 300, 65), "");
				var_name = EditorGUILayout.TextField ("Key", var_name);
				var_value = EditorGUILayout.TextField ("Value", var_value);
				GUILayout.BeginHorizontal();
				if (GUILayout.Button ("Submit") && var_name != "") {
					Vars.Add(var_name, var_value);
					show_add_variable = true;
					new_variable_time = false;
					var_name = "";
					var_value = "";
					juggle += space;
				}
				if (GUILayout.Button ("Cancel")) {
					show_add_variable = true;
					new_variable_time = false;
					var_name = "";
					var_value = "";
				}
				GUILayout.EndHorizontal();
			}
			// End add Button Code
			
			// Update the GUI
			this.Repaint();
		}
		
		#endregion
		
		#region Unity Editor Prefs
		
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
			m_muteAudio = EditorPrefs.GetBool("raconteur-m_muteAudio", false);
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
			if (m_muteAudio) {
				EditorPrefs.SetBool("raconteur-m_muteAudio", m_muteAudio);
			} else {
				EditorPrefs.DeleteKey("raconteur-m_muteAudio");
			}
		}
		
		#endregion
		
		/// <summary>
		/// Logs a string using the debugger if DebugLines is true.
		/// </summary>
		/// <param name="str">
		/// The string to log.
		/// </param>
		public static void Log(string str)
		{
			if (DebugLines) {
				Debug.Log(str);
			}
		}
	}
}

#else

namespace DPek.Raconteur
{
	public class Static
	{
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
		
		public static bool MuteAudio
		{
			get {
				return false;
			}
		}
		
		private static Dictionary<string, string> m_variables;
		public static Dictionary<string, string> Vars
		{
			get {
				m_variables = m_variables ?? new Dictionary<string, string>();
				return m_variables;
			}
		}
		
		public static void Log(string str)
		{
			// Do nothing
		}
	}
}

#endif
