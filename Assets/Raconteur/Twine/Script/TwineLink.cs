using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// A link that points to another twine passage.
	/// </summary>
	public class TwineLink : TwineLine
	{
		private string m_label;
		public string Label
		{
			get { return m_label; }
		}

		private string m_target;
		public string Target
		{
			get { return m_target; }
		}

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
		}

		public override string Print()
		{
			return m_label;
		}

		protected override string ToDebugString()
		{
			return m_target + " (" + m_label + ")";
		}
	}
}