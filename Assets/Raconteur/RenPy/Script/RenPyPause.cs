using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyPause : RenPyStatement
	{
		/// <summary>
		/// An int indicating how long to pause for.
		/// </summary>
		[SerializeField]
		private float m_time;
		public float WaitTime
		{
			get {
				return m_time;
			}
		}

		/// <summary>
		/// True if the pause is waiting for input instead of pausing for a
		/// certain amount of time.
		/// </summary>
		public bool WaitForInput
		{
			get {
				return m_time < 0;
			}
		}

		public RenPyPause() : base(RenPyStatementType.PAUSE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
		{
			tokens.Seek("pause");
			tokens.Next();
			tokens.Skip(new string[]{" ","\t"});

			// Check if there is an argument that is a number
			float time;
			if (float.TryParse (tokens.Peek(), out time)) {
				tokens.Next();
				m_time = time;
			}
			// Otherwise, this pause waits on input
			else {
				m_time = -1;
			}
		}

		public override void Execute(RenPyState state)
		{
			// Go to the next line if we are skipping the dialog
			if (Static.SkipDialog) {
				state.Execution.NextStatement(state);
			}
		}

		public override string ToDebugString()
		{
			string str = "pause";
			str += !WaitForInput ? " " + m_time : "";
			return str;
		}
	}
}
