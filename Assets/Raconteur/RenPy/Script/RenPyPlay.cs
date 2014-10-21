using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyPlay : RenPyStatement
	{
		private string m_channel;
		private string m_file;
		private bool m_loop;
		private float m_fadein;
		private float m_fadeout;

		public RenPyPlay(ref RenPyScanner tokens) : base(RenPyStatementType.PLAY)
		{
			tokens.Seek("play");
			tokens.Next();

			// Get channel and setup default looping behaviour
			tokens.Seek(new string [] {"music", "sound"});
			m_channel = tokens.Next();
			m_loop = (m_channel == "music" ? true : false);

			// TODO: support multiple play statements
			if (tokens.PeekIgnoreWhitespace(true, true, true) == "[") {
				Debug.LogError("Multiple play statement not supported yet");
			}

			// Get the filename
			tokens.Seek("\"");
			tokens.Next();
			m_file = tokens.Seek("\"");
			tokens.Next();

			// Parse any recognized clauses
			bool nothing = false;
			while (!nothing) {
				string token = tokens.PeekIgnoreWhitespace(true, true, true);
				switch (token) {
					case "loop":
						tokens.Seek("loop");
						tokens.Next();
						m_loop = true;
						break;
					case "noloop":
						tokens.Seek("noloop");
						tokens.Next();
						m_loop = false;
						break;
					case "fadein":
						tokens.Seek("fadein");
						tokens.Next();
						tokens.SkipWhitespace();
						m_fadein = float.Parse(tokens.Next());
						m_fadein = m_fadein < 0 ? 0 : m_fadein;
						break;
					case "fadeout":
						tokens.Seek("fadeout");
						tokens.Next();
						tokens.SkipWhitespace();
						m_fadeout = float.Parse(tokens.Next());
						m_fadeout = m_fadeout < 0 ? 0 : m_fadeout;
						break;
					default:
						nothing = true;
						break;
				}
			}
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Get the audio file
			if (!display.RenPyScript.HasAudioClip(m_file)) {
				Debug.LogError("Could not file AudioClip \"" + m_file + "\"");
				return;
			}
			AudioClip clip = display.RenPyScript.GetAudioClip(m_file);

			// Play the audio file
			display.BeginPlayCoroutine(m_channel, PlayAudio(display, clip));
		}

		private IEnumerator PlayAudio(RenPyDisplayState display, AudioClip clip)
		{
			AudioSource channel = GetChannel(display);

			// Fade out
			float start = channel.volume;
			float time = 0;
			if (channel.clip != clip) {
				while (time < m_fadeout) {
					yield return new WaitForFixedUpdate();
					time += Time.fixedDeltaTime;
					channel.volume = Mathf.Lerp(start, 0, time / m_fadeout);
				}
				channel.volume = 0;
			}

			// Change the audio
			channel.clip = clip;
			channel.loop = m_loop;
			channel.Play();

			// Fade in
			start = channel.volume;
			time = 0;
			while (time < m_fadein) {
				yield return new WaitForFixedUpdate();
				time += Time.fixedDeltaTime;
				channel.volume = Mathf.Lerp(start, 1, time / m_fadein);
			}
			channel.volume = 1;
		}

		private AudioSource GetChannel(RenPyDisplayState display)
		{
			switch (m_channel) {
				case "music":
					return display.Music;
				case "sound":
					return display.Sound;
			}
			return null;
		}

		public override string ToString()
		{
			string str = "play " + m_channel;
			str += " \"" + m_file + "\"";
			str += " " + (m_loop ? "loop" : "noloop");
			str += " " + (m_fadein > 0 ? "fadein " + m_fadein : "");
			str += " " + (m_fadeout > 0 ? "fadeout " + m_fadeout : "");

			str += "\n" + base.ToString();
			return str;
		}
	}
}
