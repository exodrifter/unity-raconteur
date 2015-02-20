using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Defines the start and end of a group of actions or choices.
	/// </summary>
	public class TwineGroup : TwineLine
	{
		public enum GroupType { Actions, Choices }

		private GroupType m_type;
		public GroupType Type
		{
			get { return m_type; }
		}

		private bool m_start;
		public bool Start
		{
			get { return m_start;  }
		}

		public TwineGroup(GroupType type, bool start)
		{
			m_type = type;
			m_start = start;
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			var ret = new List<TwineLine>();
			ret.Add(this);
			return ret;
		}

		public override string Print()
		{
			return "";
		}

		protected override string ToDebugString()
		{
			return "type=" + m_type + " start=" + m_start;
		}
	}
}
