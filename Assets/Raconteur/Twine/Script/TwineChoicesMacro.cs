using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwineChoiceMacro : TwineLine
	{
		private List<TwineLink> m_choices;
		public bool m_chosen;

		public TwineChoiceMacro(ref Scanner tokens)
		{
			m_choices = new List<TwineLink>();
			m_chosen = false;

			var ignore = new string[] { "<<", " ", "\n" };
			while (tokens.PeekIgnore(ignore) == "choice")
			{
				ParseChoice(ref tokens);
			}
		}

		private void ParseChoice(ref Scanner tokens)
		{
			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("choice");
			tokens.Next();

			var link = new TwineLink(ref tokens);
			m_choices.Add(link);

			tokens.Seek(">>");
			tokens.Next();
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			var list = new List<TwineLine>();
			list.Add(new TwineGroup(TwineGroup.GroupType.Choices, true));

			foreach (var choice in m_choices)
			{
				bool active = !m_chosen;
				var link = new TwineLink(choice.Label, choice.Target, active);
				link.Used += (s, e) => { m_chosen = true; };
				list.Add(link);
			}

			list.Add(new TwineGroup(TwineGroup.GroupType.Choices, false));
			return list;
		}

		public override string Print()
		{
			return null;
		}

		protected override string ToDebugString()
		{
			string ret = null;
			foreach (var choice in m_choices)
			{
				if (ret == null)
				{
					ret = choice.ToString();
				}
				else
				{
					ret += "; " + choice.ToString();
				}
			}
			return ret;
		}
	}
}
