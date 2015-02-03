using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py stop command.
	/// </summary>
	public class RenPyStop : RenPyStatement
	{
		private string m_channel;

		private float m_fadeoutTime;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyStop(ref Scanner tokens) : base(RenPyStatementType.STOP)
		{
			tokens.Seek("stop");
			tokens.Next();

			tokens.Seek(new string[] { "music", "sound", "voice" });
			m_channel = tokens.Next();

			// Parse any recognized clauses
			bool nothing = false;
			while (!nothing) {
				string token = tokens.PeekIgnore(new string[]{" ","\t","\n"});
				switch (token) {
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
			var transition = new AudioChannelTransition();
			transition.FadeTo = 0;
			transition.TransitionTime = m_fadeoutTime;
			transition.ElapsedTime = 0;

			AudioChannel channel = state.Aural.GetChannel(m_channel);
			channel.Transition = transition;
		}

		public override string ToDebugString()
		{
			string str = "stop";
			str += " " + m_channel;
			return str;
		}
	}
}
