using DPek.Raconteur.Twine.Parser;
using DPek.Raconteur.Twine.Script;

namespace DPek.Raconteur.Twine.State
{
	public class TwineExecutionState
	{
		/// <summary>
		/// The story that this execution state uses.
		/// </summary>
		private TwineStory m_story;

		/// <summary>
		/// The passage that this execution state is currently pointing to.
		/// </summary>
		private TwinePassage m_currentPassage;
		public TwinePassage CurrentPassage
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
		/// Creates a new execution state with the passed TwineStory.
		/// </summary>
		/// <param name="story">
		/// The TwineStory to keep execution state for
		/// <param>
		public TwineExecutionState(ref TwineStory story)
		{
			m_story = story;
			m_currentPassage = null;
		}

		public void GoToPassage(string name)
		{
			m_currentPassage = m_story.GetPassage(name);
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
