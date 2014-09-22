using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyCharacter : RenPyLine
	{
		private string m_varName;
		public string VarName
		{
			get {
				return m_varName;
			}
		}

		private string m_name;
		public string Name
		{
			get {
				return m_name;
			}
		}

		public RenPyCharacter(ref RenPyScanner tokens) : base(RenPyLineType.CHARACTER)
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

			// TODO: Parse character color

			// Skip the rest of the constructor
			tokens.Seek(")");
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			display.State.AddCharacter(this);
		}

		public override string ToString()
		{
			string str = "define " + m_varName + "";
			str += " = Character(";
			str += "name=\"" + m_name + "\"";
			str += ")";

			str += "\n" + base.ToString();
			return str;
		}
	}
}
