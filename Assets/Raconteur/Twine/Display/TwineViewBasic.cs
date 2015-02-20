using DPek.Raconteur.Twine.Display;
using DPek.Raconteur.Twine.Script;
using UnityEngine;
using System.Collections.Generic;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.Twine
{
	public class TwineViewBasic : MonoBehaviour
	{
		public bool m_autoStart;
		public TwineController m_controller;
		Vector2 scrollPosition = new Vector2(0, 0);

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

			if (Input.GetKeyDown(KeyCode.Escape)) {
				m_controller.StopDialog();
				scrollPosition = new Vector2(0, 0);
			}
		}

		void OnGUI()
		{
			if (!m_controller.Running) {
				return;
			}

			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.white;
			style.richText = true;

			GUILayout.BeginHorizontal();
			GUILayout.Label(m_controller.GetStoryTitle(), style);
			GUILayout.Label(m_controller.GetStoryAuthor(), style);
			GUILayout.EndHorizontal();

			var areaPad = 25;
			var areaWidth = Screen.width - areaPad*2;
			var areaHeight = Screen.height - areaPad*2;
			var areaRect = new Rect(areaPad, areaPad, areaWidth, areaHeight);
			GUILayout.BeginArea(areaRect);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition,
				GUILayout.Width(areaWidth), GUILayout.Height(areaHeight));

			bool inActionGroup = false;
			float remaining = areaWidth;
			GUILayout.BeginHorizontal();
			foreach (TwineLine line in m_controller.GetCurrentPassage())
			{
				string[] wrapped = Wrap(line.Print(), style, areaWidth, remaining, out remaining);

				if (line is TwineEcho)
				{
					for (int i = 0; i < wrapped.Length; ++i)
					{
						string str = wrapped[i];
						var layout = GUILayout.ExpandWidth(false);
						GUILayout.Label(str, style, layout);
						if (i < wrapped.Length - 1 || str.EndsWith("\n"))
						{
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
						}
					}
				}
				else if (line is TwineLink)
				{
					var link = line as TwineLink;
					if (link.Active)
					{
						style.normal.textColor = new Color(0.6f, 0.6f, 1.0f);
					}
					else
					{
						style.normal.textColor = new Color(0.5f, 0.5f, 0.5f);
					}

					for (int i = 0; i < wrapped.Length; ++i)
					{
						string str = wrapped[i];
						if (GUILayout.Button(str, style,
							GUILayout.ExpandWidth(false)) && link.Active)
						{
							m_controller.Navigate(link);
							scrollPosition = new Vector2(0, 0);
						}
						if (i < wrapped.Length - 1 || str.EndsWith("\n")
						    || inActionGroup)
						{
							GUILayout.EndHorizontal();
							GUILayout.BeginHorizontal();
						}
					}
					style.normal.textColor = Color.white;
				}
				else if (line is TwineActionGroup)
				{
					var group = line as TwineActionGroup;
					inActionGroup = group.Start;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		private string[] Wrap(string text, GUIStyle style, int width,
			float right, out float remaining)
		{
			var lines = new List<string>();
			var tokenizer = new Tokenizer(false);
			tokenizer.SetupTokens(new string[] {" ", "\n"});
			string[] tokens = tokenizer.Tokenize(ref text);
			
			remaining = right;

			string line = "";
			foreach (string token in tokens)
			{
				if (token == "\n")
				{
					lines.Add(line);
					line = "";
					continue;
				}

				string testLine = line + token;
				Vector2 size = style.CalcSize(new GUIContent(testLine));
				if ((lines.Count == 0 && size.x > right)
					|| (lines.Count != 0 && size.x > width))
				{
					lines.Add(line);
					line = token;
				}
				else
				{
					line = testLine;
				}
			}

			if (lines.Count == 0)
			{
				remaining = right - style.CalcSize(new GUIContent(line)).x;
			}
			else
			{
				remaining = width - style.CalcSize(new GUIContent(line)).x;
			}
			lines.Add(line);

			return lines.ToArray();
		}
	}
}