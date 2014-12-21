using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py image statement.
	/// </summary>
	public class RenPyImage : RenPyStatement
	{
		[SerializeField]
		private string m_imageName;
		public string ImageName
		{
			get {
				return m_imageName;
			}
		}

		[SerializeField]
		private string m_imageTag;
		public string ImageTag
		{
			get {
				return m_imageTag;
			}
		}

		[SerializeField]
		private string m_filename;
		public string Filename
		{
			get {
				return m_filename;
			}
		}

		public RenPyImage() : base(RenPyStatementType.IMAGE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref Scanner tokens)
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

		public override void Execute(RenPyState state)
		{
			string imageName = m_imageTag + " " + m_imageName;
			state.AddImageFilename(imageName, m_filename);
		}

		public override string ToDebugString()
		{
			string str = "image [" + m_imageTag;
			bool hasName = !string.IsNullOrEmpty(m_imageName);
			str += "]" + ( hasName ? " " + m_imageName : "");
			str += " = \"" + m_filename + "\"";
			return str;
		}
	}
}
