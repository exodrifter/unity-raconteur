using UnityEngine;
using System.Collections.Generic;
using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py queue statement.
	/// </summary>
	public class RenPyQueue : RenPyStatement
	{
		private string m_channel;
		private bool m_loop;
		private string[] m_files;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyQueue(ref Scanner tokens) : base(RenPyStatementType.QUEUE)
		{
			tokens.Seek("queue");
			tokens.Next();
			
			// Get channel and setup default looping behaviour
			tokens.Seek(new string [] {"music", "sound"});
			m_channel = tokens.Next();
			m_loop = (m_channel == "music" ? true : false);

			tokens.Skip(new string[] {" ", "\t", "\n"});
			
			// Get the filenames
			var files = new List<string>();
			while(true) {
				string token = tokens.PeekIgnore(new string[]{" ", "\t", "\n", "[", ","});
				if(token == "]" || token != "\"" || token == null) {
					break;
				}
				tokens.Seek("\"");
				tokens.Next();
				files.Add(tokens.Seek("\""));
				tokens.Next();
			}
			tokens.Seek("]");
			tokens.Next();
			m_files = files.ToArray();
			
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
				default:
					nothing = true;
					break;
				}
			}
		}
		
		public override void Execute(RenPyState state)
		{
			// Get the audio file
			var clips = new List<AudioClip>();
			foreach(string file in m_files) {
				if (!state.Script.HasAudioClip(file)) {
					Debug.LogError("Could not find AudioClip \"" + file + "\"");
					return;
				}
				var clip = state.Script.GetAudioClip(file);
				clips.Add(clip);
			}
			
			// Set the queue on the channel
			AudioChannel channel = state.Aural.GetChannel(m_channel);
			channel.Queue = clips;
			channel.Looping = m_loop;
		}
		
		public override string ToDebugString()
		{
			string str = "queue " + m_channel + "[";
			bool first = true;
			foreach(string file in m_files) {
				if(first) {
					str += "\"" + file + "\"";
					first = false;
				} else {
					str += ", \"" + file + "\"";
				}
			}
			str += "]";
			str += " " + (m_loop ? "loop" : "noloop");
			return str;
		}
	}
}
