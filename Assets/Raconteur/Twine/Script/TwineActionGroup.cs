using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Defines the start and end of an action group.
	/// </summary>
	public class TwineActionGroup : TwineLine
	{
		private bool m_start;
		public bool Start
		{
			get { return m_start;  }
		}

		public TwineActionGroup(bool start)
		{
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
			return "start=" + m_start;
		}
	}
}
