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
		public RenPyController m_controller;

		void Start()
		{
			if (m_autoStart) {
				m_controller.StartDialog();
			}
		}

		void Update()
		{
			if (!m_controller.Running) {
				m_controller.StopDialog();
				return;
			}

			if (m_controller.GetCurrentStatement() == null) {
				m_controller.NextStatement();
			}

			RenPyStatementType mode = m_controller.GetCurrentStatement().Type;

			switch (mode) {
				case RenPyStatementType.SAY:
					// Check for input to go to next line
					if (Input.GetMouseButtonDown(0)) {
						m_controller.NextStatement();
					}
					break;
				case RenPyStatementType.PAUSE:
					// Check for input to go to next line
					var pause = m_controller.GetCurrentStatement() as RenPyPause;
					if (pause.WaitForInput && Input.GetMouseButtonDown(0)) {
						m_controller.NextStatement();
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
					m_controller.NextStatement();
					Update(); // Update immediately to prevent delay
					break;
			}
		}

		void OnGUI()
		{
			if (!m_controller.Running || m_controller.GetCurrentStatement() == null) {
				return;
			}

			Rect rect;
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;
			style.fontSize = 15;
			style.wordWrap = true;

			// Draw background
			var bg = m_controller.GetBackgroundImage();
			if (bg != null) {
				var pos = new Rect(0, 0, Screen.width, Screen.height);
				GUI.DrawTexture(pos, bg.Texture, ScaleMode.ScaleAndCrop);
			}

			// Draw images
			var images = m_controller.GetImages();
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
			if (m_controller.ShouldDrawWindow()) {
				DrawBox(50, Screen.height - 200, Screen.width - 100, 200);
			}

			// Draw text
			switch (m_controller.GetCurrentStatement().Type) {
				case RenPyStatementType.SAY:
					var speech = m_controller.GetCurrentStatement() as RenPySay;

					// Render the speaker
					int y = Screen.height - 200;
					int width = Screen.width - 100;
					rect = new Rect(50, y, width, 200);
					style.alignment = TextAnchor.UpperLeft;
					RenPyCharacterData speaker = m_controller.GetSpeaker(speech);
					var oldColor = style.normal.textColor;
					style.normal.textColor = speaker.Color;
					GUI.Label(rect, speaker.Name, style);
					style.normal.textColor = oldColor;

					// Render the speech
					style.alignment = TextAnchor.MiddleCenter;
					rect = new Rect(50, y + 50, width, 100);
					GUI.Label(rect, speech.Text, style);
					break;

				case RenPyStatementType.MENU:
					var menu = m_controller.GetCurrentStatement() as RenPyMenu;

					// Display the choices
					int height = 30;
					int numChoices = menu.GetChoices().Count;
					int yPos = Mathf.Max(0, Screen.height/2 - numChoices*height);
					rect = new Rect(100, yPos, Screen.width-200, height);
					foreach (var choice in menu.GetChoices()) {

						// Check if a choice was selected
						DrawBox(100, rect.y + 5, rect.width, rect.height - 10);
						if (GUI.Button(rect, choice, style)) {
							m_controller.PickChoice(menu, choice);
							m_controller.NextStatement();
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
				m_controller.NextStatement();
				waiting = false;
			}
		}

		private void DrawBox(float x, float y, float width, float height)
		{
			Rect rect = new Rect(x, y, width, height);
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0, 0, new Color(0, 0, 0, 0.6f));
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(rect, GUIContent.none);
		}
	}
}
