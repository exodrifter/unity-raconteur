using DPek.Raconteur.Twine.State;
using DPek.Raconteur.Util.Expressions;
using DPek.Raconteur.Util.Parser;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	public class TwineActionsMacro : TwineLine
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
			list.Add(new TwineActionGroup(true));

			foreach (var kvp in m_actions)
			{
				var link = new TwineLink(kvp.Key, kvp.Key, kvp.Value);
				var key = kvp.Key;
				link.Used += (s, e) => { m_actions[key] = false; };
				list.Add(link);
			}

			list.Add(new TwineActionGroup(false));
			return list;
		}

		public override string Print()
		{
			return null;
		}

		protected override string ToDebugString()
		{
			string ret = null;
			foreach (var kvp in m_actions)
			{
				if (ret == null)
				{
					ret = kvp.ToString();
				}
				else
				{
					ret += "; " + kvp.ToString();
				}
			}
			return ret;
		}
	}
}
