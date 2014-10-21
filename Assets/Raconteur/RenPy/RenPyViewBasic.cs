using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;

namespace DPek.Raconteur.RenPy
{
	public class RenPyViewBasic : MonoBehaviour
	{
		public bool m_autoStart;
		public RenPyDisplayState m_display;

		void Start()
		{
			if (m_autoStart) {
				m_display.StartDialog();
			}
		}

		void Update()
		{
			if(!m_display.Running) {
				return;
			}

			RenPyLineType mode = m_display.State.CurrentLine.Type;

			switch (mode) {
				case RenPyLineType.SAY:
					// Check for input to go to next line
					if (Input.GetMouseButtonDown(0)) {
						m_display.State.NextLine(m_display);
					}
					break;
				case RenPyLineType.RETURN:
					// Stop the dialog
					if (mode == RenPyLineType.RETURN) {
						m_display.StopDialog();
					}
					break;
			}
		}

		void OnGUI()
		{
			if (!m_display.Running) {
				return;
			}

			RenPyLineType mode = m_display.State.CurrentLine.Type;

			Rect rect;
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			style.fontSize = 15;
			style.wordWrap = true;

			switch (mode) {
				case RenPyLineType.SAY:
					var speech = m_display.State.CurrentLine as RenPySay;
					if (speech == null) {
						Debug.LogError("Type mismatch!");
						m_display.State.NextLine(m_display);
					}

					// Render the speech
					int y = Screen.height - 250;
					int width = Screen.width - 100;
					rect = new Rect(50, y, width, 100);
					GUI.Label(rect, speech.Text, style);
					break;

				case RenPyLineType.MENU:
					var menu = m_display.State.CurrentLine as RenPyMenu;
					if (menu == null) {
						Debug.LogError("Type mismatch!");
						m_display.State.NextLine(m_display);
					}

					// Display the choices
					rect = new Rect(0, Screen.height - 130, Screen.width, 30);
					foreach (var choice in menu.m_choices) {

						// Check if a choice was selected
						if (GUI.Button(rect, choice.Key, style)) {
							m_display.State.GoToLabel(m_display, choice.Value);
						}

						rect.y += 30;
					}
					break;

				default:
					// Show nothing for this line, proceed to the next one.
					m_display.State.NextLine(m_display);
					break;
			}
		}
	}
}
