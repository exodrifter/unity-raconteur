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
		[SerializeField]
		private string m_channel;

		[SerializeField]
		private float m_fadeoutTime;

		public RenPyStop() : base(RenPyStatementType.STOP)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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
