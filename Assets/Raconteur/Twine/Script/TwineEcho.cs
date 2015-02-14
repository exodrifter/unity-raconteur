using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// A simple TwineLine that just returns its contents.
	/// </summary>
	public class TwineEcho : TwineLine
	{
		private string m_contents;

		public TwineEcho(ref Scanner tokens)
		{
			m_contents = tokens.Seek(new string[] { "[[", "<<", "::"});
		}

		public override List<TwineLine> Compile()
		{
			var ret = new List<TwineLine>();
			ret.Add(this);
			return ret;
		}

		public override string Print()
		{
			return m_contents;
		}

		protected override string ToDebugString()
		{
			return m_contents;
		}
	}
}
