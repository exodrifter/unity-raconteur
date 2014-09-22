using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyLabel : RenPyLine
	{
		private string m_name;
		public string Name
		{
			get {
				return m_name;
			}
		}

		public RenPyLabel(ref RenPyScanner tokens) : base(RenPyLineType.LABEL)
		{
			tokens.Seek("label");
			tokens.Next();
			m_name = tokens.Seek(":").Trim();
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "label";
			str += " " + m_name + ":";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
