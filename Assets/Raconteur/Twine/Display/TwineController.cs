using DPek.Raconteur.Twine.Parser;
using DPek.Raconteur.Twine.Script;
using DPek.Raconteur.Twine.State;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace DPek.Raconteur.Twine.Display
{
	/// <summary>
	/// Interface for a TwineView to interact with. Manages running a TwineStory
	/// as well as all of the relevant GameObject references created by the
	/// script.
	/// </summary>
	public sealed class TwineController : MonoBehaviour
	{
		/// <summary>
		/// The script that the display will run.
		/// </summary>
		[SerializeField]
		private TwineScriptAsset m_twineScriptAsset;
		public TwineScriptAsset Asset
		{
			get {
				return m_twineScriptAsset;
			}
		}

		/// <summary>
		/// The parsed script.
		/// </summary>
		private TwineStory m_story;

		/// <summary>
		/// The state of the script's execution.
		/// </summary>
		private TwineState m_state;

		/// <summary>
		/// Whether or not the display is running the script
		/// </summary>
		private bool running = false;
		public bool Running
		{
			get {
				return running && m_state != null && m_state.Execution.Running;
			}
		}

		/// <summary>
		/// Creates all of the GameObjects that the script will need to run.
		/// </summary>
		public void Awake()
		{
			if (m_twineScriptAsset == null) {
				throw new System.Exception("Script property is not set!");
			}

			// Parse the script
			m_story = TwineParser.Parse(m_twineScriptAsset.content);
			m_state = new TwineState(ref m_story);
		}

		/// <summary>
		/// Starts the script from the beginning.
		/// </summary>
		public void StartDialog()
		{
			StopAllCoroutines();
			running = true;

			m_state.Reset();
			m_state.Execution.GoToPassage("Start");
		}

		/// <summary>
		/// Stops the script.
		/// </summary>
		public void StopDialog()
		{
			running = false;
		}

		public string GetStoryTitle()
		{
			return m_story.Title;
		}

		public string GetStoryAuthor()
		{
			return m_story.Author;
		}

		#region Execution Wrapper Methods

		/// <summary>
		/// Returns the current TwinePassage.
		/// </summary>
		/// <returns>
		/// The current TwinePassage.
		/// </returns>
		public List<TwineLine> GetCurrentPassage()
		{
			return m_state.Execution.CurrentPassage;
		}

		/// <summary>
		/// Changes the current passage to the specified passage. This should
		/// NOT be used to jump to a passage as a result of clicking a link. To
		/// do that, call <see cref="Navigate"/> instead.
		/// </summary>
		/// <param name="title">
		/// The passage to go to.
		/// </param>
		public void GoToPassage(string title)
		{
			m_state.Execution.GoToPassage(title);
		}

		/// <summary>
		/// Navigates to a passage as specified by a link
		/// </summary>
		/// <param name="title">
		/// The passage to go to.
		/// </param>
		public void Navigate(TwineLink link)
		{
			if (!link.Active)
			{
				throw new InvalidOperationException("Link \"" + link.Label
				    + "\"=>(" + link.Target + ") is not active");
			}

			link.OnUsed();
			m_state.Execution.GoToPassage(link.Target);
		}

		#endregion
	}
}
