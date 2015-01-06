using DPek.Raconteur.Twine.Parser;
using DPek.Raconteur.Twine.Script;

namespace DPek.Raconteur.Twine.State
{
	/// <summary>
	/// Stores the state of a Twine story.
	/// </summary>
	public class TwineState
	{
		/// <summary>
		/// The Twine story to store state for.
		/// </summary>
		private TwineStory m_story;
		public TwineStory Script
		{
			get { return m_story; }
		}

		/// <summary>
		/// Stores the execution state of the Twine story.
		/// </summary>
		private TwineExecutionState m_executionState;
		public TwineExecutionState Execution
		{
			get { return m_executionState; }
		}

		/// <summary>
		/// Creates a new TwineState for the passed TwineStory.
		/// </summary>
		/// <param name="script">
		/// The script to store state for.
		/// </param>
		public TwineState(ref TwineStory story)
		{
			m_story = story;
			m_executionState = new TwineExecutionState(ref story);
		}

		/// <summary>
		/// Resets the state.
		/// </summary>
		public void Reset()
		{
			m_executionState.Reset();
		}

		/// <summary>
		/// Gets the value of the specified variable.
		/// </summary>
		/// <returns>
		/// The value of the specified variable or an empty string if the
		/// variable does not exist.
		/// </returns>
		/// <param name="name">
		/// The name of the variable to get the value of.
		/// </param>
		public string GetVariable(string name)
		{
			return Static.Vars.ContainsKey(name) ? Static.Vars[name] : "";
		}

		/// <summary>
		/// Sets the specified variable to the specified value.
		/// </summary>
		/// <param name="name">
		/// The name of the variable to set the value of.
		/// </param>
		/// <param name="value">
		/// The value to the variable to.
		/// </param>
		public void SetVariable(string name, string value)
		{
			if (!Static.Vars.ContainsKey(name)) {
				Static.Vars.Add(name, value);
			} else {
				Static.Vars[name] = value;
			}
		}
	}
}
