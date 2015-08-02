using UnityEngine;
using System.Collections.Generic;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyCall : RenPyStatement
	{
		private string m_label;

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyCall(ref Scanner tokens) : base(RenPyStatementType.CALL)
		{
			tokens.Seek("call");
			tokens.Next();
			tokens.Skip(new string[]{" ","\t"});

			// Get the label that we want to call
			m_label = tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			var frame = state.Execution.InitialStackFrame.Blocks;
			state.Execution.PushStackFrame(frame);
			state.Execution.GoToLabel(m_label);
		}
		
		public override string ToDebugString()
		{
			string str = "call " + m_label;
			return str;
		}
	}
}
