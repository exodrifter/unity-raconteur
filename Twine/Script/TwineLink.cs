using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Parser;
using System;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// A link that points to another Twine passage.
	/// </summary>
	public class TwineLink : TwineLine
	{
		/// <summary>
		/// The text content of the link.
		/// </summary>
		private string m_label;
		public string Label
		{
			get { return m_label; }
		}

		/// <summary>
		/// The passage this link targets.
		/// </summary>
		private string m_target;
		public string Target
		{
			get { return m_target; }
		}

		/// <summary>
		/// Whether or not the link can be clicked.
		/// </summary>
		private bool m_active;
		public bool Active
		{
			get { return m_active; }
		}

		/// <summary>
		/// Event for when the link gets used.
		/// </summary>
		public event EventHandler Used;

		public TwineLink(ref Scanner tokens)
		{
			tokens.Seek("[[");
			tokens.Next();

			m_label = tokens.Seek(new string[] { "]]", "|"}).Trim();

			if (tokens.Next() == "|") {
				m_target = tokens.Seek("]]").Trim();
				tokens.Next();
			} else {
				m_target = m_label;
			}

			m_active = true;
		}

		public TwineLink(string label, string target, bool active)
		{
			m_label = label;
			m_target = target;
			m_active = active;
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			var ret = new List<TwineLine>();
			ret.Add(this);
			return ret;
		}

		public override string Print()
		{
			return m_label;
		}

		public void OnUsed()
		{
			if (Used != null)
			{
				Used(this, null);
			}
		}

		protected override string ToDebugString()
		{
			string str = "link ";
			str += "active=" + m_active + " ";
			str += "label=" + m_label + " ";
			str += "target=" + m_target + " ";
			return str;
		}
	}
}