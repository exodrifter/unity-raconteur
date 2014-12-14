using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.Display;
using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.Script;
using DPek.Raconteur.RenPy.State;
using System.Collections;

namespace DPek.Raconteur.RenPy
{
	public class RenPyViewBasic : MonoBehaviour
	{
		public bool m_autoStart;
		public RenPyDisplay m_display;

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

			if (m_display.GetCurrentStatement() == null) {
				m_display.NextStatement();
			}

			RenPyStatementType mode = m_display.GetCurrentStatement().Type;

			switch (mode) {
				case RenPyStatementType.SAY:
					// Check for input to go to next line
					if (Input.GetMouseButtonDown(0)) {
						m_display.NextStatement();
					}
					break;
				case RenPyStatementType.PAUSE:
					// Check for input to go to next line
					var pause = m_display.GetCurrentStatement() as RenPyPause;
					if (pause.WaitForInput && Input.GetMouseButtonDown(0)) {
						m_display.NextStatement();
					}
					// Or wait until we can go to the next line
					else {
						StartCoroutine(WaitNextStatement(pause.WaitTime));
					}
					break;
				case RenPyStatementType.MENU:
					// Do nothing
					break;
				default:
					// Show nothing for this line, proceed to the next one.
					m_display.NextStatement();
					break;
			}
		}

		void OnGUI()
		{
			if (!m_display.Running || m_display.GetCurrentStatement() == null) {
				return;
			}

			Rect rect;
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			style.fontSize = 15;
			style.wordWrap = true;

			// Draw background
			var bg = m_display.GetBackgroundImage();
			if (bg != null) {
				var pos = new Rect(0, 0, Screen.width, Screen.height);
				GUI.DrawTexture(pos, bg.Texture, ScaleMode.ScaleAndCrop);
			}

			// Draw images
			var images = m_display.GetImages();
			foreach (RenPyImageData image in images) {
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

			// Draw the window if needed
			if (m_display.ShouldDrawWindow()) {
				Texture2D texture = new Texture2D(1, 1);
				texture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
				texture.Apply();
				GUI.skin.box.normal.background = texture;
				Rect dim = new Rect(50, Screen.height - 200,
									Screen.width - 100, 200);
				GUI.Box(dim, GUIContent.none);
			}

			// Draw text
			switch (m_display.GetCurrentStatement().Type) {
				case RenPyStatementType.SAY:
					var speech = m_display.GetCurrentStatement() as RenPySay;
					if (speech == null) {
						Debug.LogError("Type mismatch!");
						m_display.NextStatement();
					}

					// Render the speaker
					int y = Screen.height - 200;
					int width = Screen.width - 100;
					rect = new Rect(50, y, width, 200);
					style.alignment = TextAnchor.UpperLeft;
					string speaker = speech.Speaker;
					if (speech.Speaker != null)
					{
						if (speech.SpeakerIsVariable) {
							var ch = m_display.GetCharacter(speaker);
							var oldColor = style.normal.textColor;
							style.normal.textColor = ch.Color;
							GUI.Label(rect, ch.Name, style);
							style.normal.textColor = oldColor;
						}
						else {
							GUI.Label(rect, speaker, style);
						}
					}

					// Render the speech
					style.alignment = TextAnchor.MiddleCenter;
					rect = new Rect(50, y + 50, width, 100);
					GUI.Label(rect, speech.Text, style);
					break;

				case RenPyStatementType.MENU:
					var menu = m_display.GetCurrentStatement() as RenPyMenu;
					if (menu == null) {
						Debug.LogError("Type mismatch!");
						m_display.NextStatement();
					}

					// Display the choices
					int height = 30;
					int numChoices = menu.GetChoices().Count;
					int yPos = Mathf.Max(0, Screen.height/2 - numChoices*height);
					rect = new Rect(0, yPos, Screen.width, height);
					foreach (var choice in menu.GetChoices()) {

						// Check if a choice was selected
						if (GUI.Button(rect, choice, style)) {
							m_display.PickChoice(menu, choice);
							m_display.NextStatement();
						}

						rect.y += height;
					}
					break;
			}
		}

		bool waiting = false;
		private IEnumerator WaitNextStatement(float time)
		{
			if (!waiting) {
				waiting = true;
				yield return new WaitForSeconds(time);
				m_display.NextStatement();
				waiting = false;
			}
		}
	}
}
