using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyReturn : RenPyLine
	{
		public RenPyReturn(ref RenPyScanner tokens) : base(RenPyLineType.RETURN)
		{
			tokens.Seek("return");
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			display.StopDialog();
		}

		public override string ToString()
		{
			string str = "return";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
