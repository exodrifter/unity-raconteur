using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py character statement.
	/// </summary>
	public class RenPyCharacter : RenPyStatement
	{
		/// <summary>
		/// The name of the variable that stores this character.
		/// </summary>
		private string m_varName;
		public string VarName
		{
			get {
				return m_varName;
			}
		}

		/// <summary>
		/// The name of this character.
		/// </summary>
		private string m_name;
		public string Name
		{
			get {
				return m_name;
			}
		}

		private Color m_color;
		public Color Color
		{
			get {
				return m_color;
			}
		}

		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyCharacter(ref Scanner tokens)
			: base(RenPyStatementType.CHARACTER)
		{
			tokens.Seek("define");
			tokens.Next();

			// Get the variable name of the character
			m_varName = tokens.Seek("=").Trim();
			tokens.Next();

			// Get to the constructor
			tokens.Seek("(");
			tokens.Next();

			// Get the character's name
			tokens.Seek(new string[] {"\"", "\'"});
			string quote = tokens.Next();
			m_name = tokens.Seek(quote);
			tokens.Next();

			// Parse character color
			tokens.Seek(new string[] {"(", ")", "\"", "\'"});
			if(tokens.Peek() != ")") {
				quote = tokens.Next();
				if(quote == "(") {
					float r, g, b = 0;
					float a = 1;
					float.TryParse(tokens.Next(), out r);
					r /= 255;
					tokens.Next();
					tokens.Skip(new string[] {" "});
					float.TryParse(tokens.Next(), out g);
					g /= 255;
					tokens.Next();
					tokens.Skip(new string[] {" "});
					float.TryParse(tokens.Next(), out b);
					b /= 255;
					tokens.Next();
					tokens.Skip(new string[] {" "});
					float.TryParse(tokens.Next(), out a);
					a /= 255;
					tokens.Seek(")");
					tokens.Next();
					m_color = new Color(r,g,b,a);
				} else {
					string colorHex = tokens.Seek(quote);
					tokens.Next();
					m_color = ColorHexConverter.FromRGB(colorHex);
				}
			}

			// Skip the rest of the constructor
			tokens.Seek(")");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.AddCharacter(this);
		}

		public override string ToDebugString()
		{
			string str = "define " + m_varName + "";
			str += " = Character(";
			str += "name=\"" + m_name + "\", ";
			str += "color=\"" + m_color + "\"";
			str += ")";
			return str;
		}
	}
}
