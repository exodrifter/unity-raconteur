using DPek.Raconteur.Twine.Parser;
using DPek.Raconteur.Twine.Script;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.State
{
	public class TwineExecutionState
	{
		/// <summary>
		/// The story that this execution state uses.
		/// </summary>
		private TwineState m_state;

		/// <summary>
		/// The passage that this execution state is currently pointing to.
		/// </summary>
		private List<TwineLine> m_currentPassage;
		public List<TwineLine> CurrentPassage
		{
			get { return m_currentPassage; }
		}

		/// <summary>
		/// Whether or not the state is currently running.
		/// </summary>
		public bool Running
		{
			get { return m_currentPassage != null; }
		}

		/// <summary>
		/// Creates a new execution state.
		/// </summary>
		/// <param name="state">
		/// The TwineState this execution state belongs to.
		/// <param>
		public TwineExecutionState(TwineState state)
		{
			m_state = state;
			m_currentPassage = null;
		}

		public void GoToPassage(string name)
		{
			Static.Log("<b>Starting compilation of \"" + name + "\"...</b>\n");
			TwinePassage passage = m_state.Script.GetPassage(name);
			m_currentPassage = passage.Compile(m_state);
			Static.Log("<b>Finished compilation of \"" + name + "\"</b>\n");
		}

		/// <summary>
		/// Resets the state of the TwineExecutionState.
		/// </summary>
		public void Reset()
		{
			m_currentPassage = null;
		}
	}
}
