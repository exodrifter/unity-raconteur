using UnityEngine;
using DPek.Raconteur.RenPy.Util;

namespace DPek.Raconteur.RenPy.Dialog
{
	public class RenPyDialogImage
	{
		/// <summary>
		/// The texture for this image.
		/// </summary>
		private Texture2D m_texture;
		public Texture2D Texture
		{
			get
			{
				return m_texture;
			}
			set
			{
				m_texture = value;
			}
		}

		/// <summary>
		/// The alignment of this image.
		/// </summary>
		private RenPyAlignment m_alignment;
		public RenPyAlignment Alignment
		{
			get
			{
				return m_alignment;
			}
			set
			{
				m_alignment = value;
			}
		}

		public RenPyDialogImage(ref Texture2D texture, RenPyAlignment alignment)
		{
			m_texture = texture;
			m_alignment = alignment;
		}
	}
}
