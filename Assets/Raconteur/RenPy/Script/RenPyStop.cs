using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyStop : RenPyStatement
	{
		private string m_channel;

		private float m_fadeout;

		public RenPyStop(ref RenPyScanner tokens) : base(RenPyStatementType.STOP)
		{
			tokens.Seek("stop");
			tokens.Next();

			tokens.Seek(new string [] {"music", "sound"});
			m_channel = tokens.Next();

			// Parse any recognized clauses
			bool nothing = false;
			while (!nothing) {
				string token = tokens.PeekIgnoreWhitespace(true, true, true);
				switch (token) {
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
			string msg = "stop " + m_channel;
			msg += (m_fadeout > 0 ? " fadeout:" + m_fadeout : "");
			Static.Log(msg);

			// Stop the audio
			display.BeginPlayCoroutine(m_channel, StopAudio(display));
		}

		private IEnumerator StopAudio(RenPyDisplayState display)
		{
			AudioSource channel =  GetChannel(display);

			// Fade out
			float time = 0;
			while (time < m_fadeout) {
				yield return new WaitForFixedUpdate();
				time += Time.fixedDeltaTime;
				channel.volume = Mathf.Lerp(1, 0, time / m_fadeout);
			}

			// Change the audio
			channel.volume = 0;
			channel.clip = null;
			channel.Stop();
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
			string str = "stop";
			str += " " + m_channel;

			str += "\n" + base.ToString();
			return str;
		}
	}
}
