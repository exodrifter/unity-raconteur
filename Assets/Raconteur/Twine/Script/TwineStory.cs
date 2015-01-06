using UnityEngine;
using System;
using System.Collections.Generic;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Represents a story in Twine.
	/// </summary>
	public class TwineStory
	{
		/// <summary>
		/// The title of the story.
		/// </summary>
		private string m_title;
		public string Title {
			get { return m_title;  }
			set { m_title = value; }
		}

		/// <summary>
		/// The author of the story.
		/// </summary>
		private string m_author;
		public string Author
		{
			get { return m_author; }
			set { m_author = value; }
		}

		/// <summary>
		/// A dictionary of passage titles to passages.
		/// </summary>
		private Dictionary<string, TwinePassage> m_passages;

		/// <summary>
		/// Creates a new, empty Twine story.
		/// </summary>
		public TwineStory() {
			m_title = null;
			m_author = null;
			m_passages = new Dictionary<string, TwinePassage>();
		}

		/// <summary>
		/// Adds the specified passage to the story.
		/// </summary>
		/// <param name="TwinePassage">
		/// The TwinePassage to add to the story.
		/// </param>
		public void AddPassage(TwinePassage passage) {
			if (m_passages.ContainsKey(passage.Title)) {
				throw new ArgumentException("Story already contains a passage "
					+ "with the title \"" + passage.Title + "\"");
			} else {
				m_passages.Add(passage.Title, passage);
			}
		}

		/// <summary>
		/// Returns the specified passage.
		/// </summary>
		/// <param name="passageTitle">The title of the passage to get</param>
		/// <returns></returns>
		public TwinePassage GetPassage(string passageTitle) {
			return m_passages[passageTitle];
		}
	}
}
