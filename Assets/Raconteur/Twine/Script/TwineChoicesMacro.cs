using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine actions macro is a set of links that the player can choose
	/// from. Choosing one link will disable the other links.
	/// </summary>
	public class TwineChoiceMacro : TwineMacro
	{
		/// <summary>
		/// The set of choices to choose from.
		/// </summary>
		private List<TwineLink> m_choices;

		/// <summary>
		/// Whether or not a choice has been chosen.
		/// </summary>
		public bool m_chosen;

		public TwineChoiceMacro(ref Scanner tokens)
		{
			m_choices = new List<TwineLink>();
			m_chosen = false;

			var ignore = new string[] { "<<", " ", "\n" };
			while (tokens.PeekIgnore(ignore) == "choice")
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
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			var list = new List<TwineLine>();
			list.Add(new TwineGroup(TwineGroup.GroupType.CHOICES, true));

			foreach (var choice in m_choices)
			{
				bool active = !m_chosen;
				var link = new TwineLink(choice.Label, choice.Target, active);
				link.Used += (s, e) => { m_chosen = true; };
				list.Add(link);
			}

			list.Add(new TwineGroup(TwineGroup.GroupType.CHOICES, false));
			return list;
		}

		protected override string ToDebugString()
		{
			string str = "choices ";
			str += "enabled=" + !m_chosen + " ";
			
			bool first = true;
			foreach (var link in m_choices)
			{
				if(!first)
				{
					str += ", ";
				}
				
				str += "[[" + link.Label + "|" + link.Target + "]]";
				first = false;
			}
			return str;
		}
	}
}
