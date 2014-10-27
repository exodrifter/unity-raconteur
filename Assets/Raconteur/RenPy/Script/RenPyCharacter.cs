using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

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

		public RenPyCharacter(ref RenPyScanner tokens)
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

			// TODO: Parse character color

			// Skip the rest of the constructor
			tokens.Seek(")");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			state.AddCharacter(this);
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
