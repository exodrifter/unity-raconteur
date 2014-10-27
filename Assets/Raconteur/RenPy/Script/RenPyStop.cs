using UnityEngine;
using System.Collections;
using System.IO;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py stop command.
	/// </summary>
	public class RenPyStop : RenPyStatement
	{
		private string m_channel;

		private float m_fadeoutTime;

		public RenPyStop(ref RenPyScanner tokens) : base(RenPyStatementType.STOP)
		{
			tokens.Seek("stop");
			tokens.Next();

			tokens.Seek(new string[] { "music", "sound", "voice" });
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

		public override string ToString()
		{
			string str = "stop";
			str += " " + m_channel;

			str += "\n" + base.ToString();
			return str;
		}
	}
}
