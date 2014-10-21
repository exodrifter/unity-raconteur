using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyImage : RenPyStatement
	{
		private string m_imageName;
		public string ImageName
		{
			get {
				return m_imageName;
			}
		}

		private string m_imageTag;
		public string ImageTag
		{
			get {
				return m_imageTag;
			}
		}

		private string m_filename;
		public string Filename
		{
			get {
				return m_filename;
			}
		}

		public RenPyImage(ref RenPyScanner tokens) : base(RenPyStatementType.IMAGE)
		{
			tokens.Seek("image");
			tokens.Next();

			string m_varName = tokens.Seek("=").Trim();
			string[] parts = m_varName.Split(' ');
			m_imageTag = parts[0];
			m_imageName = "";
			for (int i = 1; i < parts.Length; ++i) {
				m_imageName += parts[i];
			}
			tokens.Next();

			tokens.Seek(new string[] { "\"", "\'" });
			tokens.Next();
			m_filename = tokens.Next();
			tokens.Seek(new string[] {"\"", "\'"});
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			string imageName = m_imageTag + " " + m_imageName;
			display.State.AddImageFilename(imageName, m_filename);
		}

		public override string ToString()
		{
			string str = "image [" + m_imageTag;
			bool hasName = !string.IsNullOrEmpty(m_imageName);
			str += "]" + ( hasName ? " " + m_imageName : "");
			str += " = \"" + m_filename + "\"";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
