using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyImage : RenPyLine
	{
		private string m_varName;
		public string VarName
		{
			get {
				return m_varName;
			}
		}

		private string m_imageName;
		public string ImageName
		{
			get {
				return m_imageName;
			}
		}

		public RenPyImage(ref RenPyScanner tokens) : base(RenPyLineType.IMAGE)
		{
			tokens.Seek("image");
			tokens.Next();
			m_varName = tokens.Seek("=").Trim();
			tokens.Next();

			tokens.Seek(new string[] {"\"", "\'"});
			m_imageName = tokens.Next();
			tokens.Seek(new string[] {"\"", "\'"});
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			// Nothing to do
		}

		public override string ToString()
		{
			string str = "image " + m_varName;
			str += " = \"" + m_imageName + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
