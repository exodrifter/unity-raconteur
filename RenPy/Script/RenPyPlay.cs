using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py play statement.
	/// </summary>
	public class RenPyPlay : RenPyStatement
	{
		private string m_channel;
		private string m_file;
		private bool m_loop;
		private bool m_ifChanged;
		private float m_fadeinTime;
		private float m_fadeoutTime;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyPlay(ref Scanner tokens) : base(RenPyStatementType.PLAY)
		{
			tokens.Seek("play");
			tokens.Next();

			// Get channel and setup default looping behaviour
			tokens.Seek(new string [] {"music", "sound", "voice"});
			m_channel = tokens.Next();
			m_loop = (m_channel == "music" ? true : false);

			// TODO: support multiple play statements
			if (tokens.PeekIgnore(new string[]{" ","\t","\n"}) == "[") {
				Debug.LogError("Multiple play statement not supported yet");
			}

			// Get the filename
			tokens.Seek("\"");
			tokens.Next();
			m_file = tokens.Seek("\"");
			tokens.Next();

			// Default clause values
			m_loop = false;
			m_fadeinTime = 0;
			m_fadeoutTime = 0;
			m_ifChanged = false;

			// Parse any recognized clauses
			bool nothing = false;
			while (!nothing) {
				string token = tokens.PeekIgnore(new string[]{" ","\t","\n"});
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
					case "if_changed":
						tokens.Seek("if_changed");
						tokens.Next();
						m_ifChanged = true;
						break;
					case "fadein":
						tokens.Seek("fadein");
						tokens.Next();
						tokens.Skip(new string[]{" ","\t"});
						m_fadeinTime = float.Parse(tokens.Next());
						m_fadeinTime = m_fadeinTime < 0 ? 0 : m_fadeinTime;
						break;
					case "fadeout":
						tokens.Seek("fadeout");
						tokens.Next();
						tokens.Skip(new string[]{" ","\t"});
						m_fadeoutTime = float.Parse(tokens.Next());
						m_fadeoutTime = m_fadeoutTime < 0 ? 0 : m_fadeoutTime;
						break;
					default:
						nothing = true;
						break;
				}
			}
		}

		public override void Execute(RenPyState state)
		{
			// Get the audio file
			if (!state.Script.HasAudioClip(m_file)) {
				Debug.LogError("Could not find AudioClip \"" + m_file + "\"");
				return;
			}
			AudioClip clip = state.Script.GetAudioClip(m_file);

			// Check if the audio file is the same
			AudioChannel channel = state.Aural.GetChannel(m_channel);
			if (m_ifChanged && channel.Clip == clip) {
				return;
			}

			// Setup the transitions
			var fadein = new AudioChannelTransition();
			fadein.FadeTo = 1;
			fadein.ElapsedTime = m_fadeinTime;
			fadein.Loop = m_loop;

			var fadeout = new AudioChannelTransition();
			fadeout.EndAudio = clip;
			fadeout.FadeTo = 0;
			fadeout.TransitionTime = m_fadeoutTime;
			fadeout.Loop = m_loop;
			fadeout.NextTransition = fadein;

			channel.Transition = fadeout;
		}

		public override string ToDebugString()
		{
			string str = "play " + m_channel;
			str += " \"" + m_file + "\"";
			str += " " + (m_loop ? "loop" : "noloop");
			str += " " + (m_fadeinTime > 0 ? "fadein " + m_fadeinTime : "");
			str += " " + (m_fadeoutTime > 0 ? "fadeout " + m_fadeoutTime : "");
			return str;
		}
	}
}
