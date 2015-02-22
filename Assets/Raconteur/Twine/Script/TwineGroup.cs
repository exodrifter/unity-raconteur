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
		public enum GroupType { ACTIONS, CHOICES }

		private GroupType m_type;
		public GroupType Type
		{
			get { return m_type; }
		}

		private int m_itemCount;
		public int ItemCount
		{
			get { return m_itemCount; }
		}

		public TwineGroup(GroupType type, int itemCount)
		{
			m_type = type;
			m_itemCount = itemCount;
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
			string str = "group ";
			str += "itemCount=" + m_itemCount;
			str += "type=" + m_type + " ";
			return str;
		}
	}
}
