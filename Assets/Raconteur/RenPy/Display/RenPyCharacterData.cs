using UnityEngine;

namespace DPek.Raconteur.RenPy.Display
{
	/// <summary>
	/// A non-scriptable object that contains data about a RenPyCharacter. Used
	/// to transfer information about a character from a RenPyScript to a
	/// RenPyView.
	/// </summary>
	public class RenPyCharacterData
	{
		/// <summary>
		/// The name of the character.
		/// </summary>
		private string m_name;
		public string Name
		{
			get
			{
				return m_name;
			}
		}

		/// <summary>
		/// The color of the character.
		/// </summary>
		private Color m_color;
		public Color Color
		{
			get
			{
				return m_color;
			}
		}

		/// <summary>
		/// Creates a new RenPyCharacterData with the specified properties.
		/// </summary>
		/// <param name="name">
		/// The name of the character.
		/// </param>
		/// <param name="color">
		/// The color of the character.
		/// </param>
		public RenPyCharacterData(string name, Color color)
		{
			m_name = name;
			m_color = color;
		}
	}
}
