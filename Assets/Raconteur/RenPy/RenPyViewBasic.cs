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

			RenPyStatementType mode = m_display.State.CurrentLine.Type;

			switch (mode) {
				case RenPyStatementType.SAY:
					// Check for input to go to next line
					if (Input.GetMouseButtonDown(0)) {
						m_display.State.NextLine(m_display);
					}
					break;
				case RenPyStatementType.RETURN:
					// Stop the dialog
					if (mode == RenPyStatementType.RETURN) {
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

			RenPyStatementType mode = m_display.State.CurrentLine.Type;

			Rect rect;
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			style.fontSize = 15;
			style.wordWrap = true;

			// Draw images
			var imageNames = m_display.State.GetImages();
			foreach (Dialog.RenPyDialogImage image in imageNames) {
				float screenWidth = Screen.width;
				float screenHeight = Screen.height;
				float texWidth = image.Texture.width;
				float texHeight = image.Texture.height;
				var pos = new Rect(0, 0, texWidth, texHeight);
				switch(image.Alignment) {
					case Util.RenPyAlignment.BottomCenter:
						pos.x = screenWidth / 2 - texWidth / 2;
						pos.y = screenHeight - texHeight;
						break;
					case Util.RenPyAlignment.BottomLeft:
						pos.x = 0;
						pos.y = screenHeight - texHeight;
						break;
					case Util.RenPyAlignment.BottomRight:
						pos.x = screenWidth - texWidth;
						pos.y = screenHeight - texHeight;
						break;
					case Util.RenPyAlignment.Center:
						pos.x = screenWidth / 2 - texWidth / 2;
						pos.y = screenHeight / 2 - texHeight / 2;
						break;
					case Util.RenPyAlignment.LeftCenter:
						pos.x = 0;
						pos.y = screenHeight / 2 - texHeight / 2;
						break;
					case Util.RenPyAlignment.RightCenter:
						pos.x = screenHeight - texWidth;
						pos.y = screenHeight / 2 - texHeight / 2;
						break;
					case Util.RenPyAlignment.TopCenter:
						pos.x = screenWidth / 2 - texWidth / 2;
						pos.y = 0;
						break;
					case Util.RenPyAlignment.TopLeft:
						pos.x = 0;
						pos.y = 0;
						break;
					case Util.RenPyAlignment.TopRight:
						pos.x = screenHeight - texWidth;
						pos.y = 0;
						break;
				}
				GUI.DrawTexture(pos, image.Texture, ScaleMode.ScaleToFit);
			}

			// Draw text
			switch (mode) {
				case RenPyStatementType.SAY:
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

				case RenPyStatementType.MENU:
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
