using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// The Twine actions macro is a set of links that the player can choose
	/// from. Each link can only be chosen once.
	/// </summary>
	public class TwineActionsMacro : TwineMacro
	{
		private Dictionary<string, bool> m_actions;

		public TwineActionsMacro(ref Scanner tokens)
		{
			m_actions = new Dictionary<string, bool>();

			tokens.Seek("<<");
			tokens.Next();
			tokens.Seek("actions");
			tokens.Next();

			while (tokens.PeekIgnore(new string[] {" "}) != ">>")
			{
				string quote;
				tokens.Seek(new string[] {"\"", "'"}, out quote);
				tokens.Next();

				string action = tokens.Seek(quote);
				m_actions.Add(action, true);
				tokens.Next();
			}

			tokens.Seek(">>");
			tokens.Next();
		}

		public override List<TwineLine> Compile(TwineState state)
		{
			var list = new List<TwineLine>();
			list.Add(new TwineGroup(TwineGroup.GroupType.ACTIONS, m_actions.Count));

			foreach (var kvp in m_actions)
			{
				var link = new TwineLink(kvp.Key, kvp.Key, kvp.Value);
				var key = kvp.Key;
				link.Used += (s, e) => { m_actions[key] = false; };
				list.Add(link);
			}

			return list;
		}

		protected override string ToDebugString()
		{
			string str = "actions ";

			bool first = true;
			foreach (var kvp in m_actions)
			{
				if(!first)
				{
					str += ", ";
				}

				str += "[link=\"" + kvp.Key + "\" enabled=" + kvp.Value + "]";
				first = false;
			}
			return str;
		}
	}
}
