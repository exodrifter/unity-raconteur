using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py play statement.
	/// </summary>
	public class RenPyPlay : RenPyStatement
	{
		[SerializeField]
		private string m_channel;
		[SerializeField]
		private string m_file;
		[SerializeField]
		private bool m_loop;
		[SerializeField]
		private float m_fadeinTime;
		[SerializeField]
		private float m_fadeoutTime;

		public RenPyPlay() : base(RenPyStatementType.PLAY)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("play");
			tokens.Next();

			// Get channel and setup default looping behaviour
			tokens.Seek(new string [] {"music", "sound"});
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
			if (!state.Data.HasAudioClip(m_file)) {
				Debug.LogError("Could not file AudioClip \"" + m_file + "\"");
				return;
			}
			AudioClip clip = state.Data.GetAudioClip(m_file);

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

			AudioChannel channel = state.Aural.GetChannel(m_channel);
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
