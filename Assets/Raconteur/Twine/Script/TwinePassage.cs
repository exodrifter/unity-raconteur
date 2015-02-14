using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Parser;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Represents a passage in Twine.
	/// </summary>
	public class TwinePassage
	{
		#region Properties
		
		/// <summary>
		/// The title of this passage
		/// </summary>
		private string m_title;
		public string Title
		{
			get { return m_title; }
		}

		/// <summary>
		/// The tags of this passage
		/// </summary>
		private string[] m_tags;
		public string[] Tags
		{
			get { return m_tags; }
		}

		/// <summary>
		/// The source lines of this passage
		/// </summary>
		private List<TwineLine> m_source;
		public List<TwineLine> Source
		{
			get { return m_source; }
		}

		#endregion
		
		/// <summary>
		/// Parses a Twine passage from a scanner
		/// </summary>
		/// <param name="tokens">
		/// The tokens to parse.
		/// </param>
		public TwinePassage(ref Scanner tokens)
		{
			tokens.Seek("::");
			tokens.Next();

			m_title = tokens.Seek(new string[] { "\n", "[" }).Trim();

			var tags = new List<string>();
			if (tokens.Next() == "[")
			{
				string tag = tokens.Next();
				while (tag != "]")
				{
					if(!string.IsNullOrEmpty(tag))
					{
						tags.Add(tag);
					}
					tokens.Skip(new string[] { " ", "\t", "\n" });
					tag = tokens.Next();
				}

				tokens.Seek("\n");
				tokens.Next();
			}
			m_tags = tags.ToArray();

			m_source = new List<TwineLine>();
		}

		public List<TwineLine> Compile(TwineState state)
		{
			var ret = new List<TwineLine>();
			foreach (TwineLine line in m_source)
			{
				ret.AddRange(line.Compile(state));
			}
			return ret;
		}
	}
}
