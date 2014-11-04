using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyVariable : RenPyStatement
	{
		[SerializeField]
		private string m_varName;
		public string VarName
		{
			get {
				return m_varName;
			}
		}

		[SerializeField]
		private string m_value;
		public string Value
		{
			get {
				return m_value;
			}
		}

		[SerializeField]
		private Assignment m_assignment;
		public Assignment Operator
		{
			get {
				return m_assignment;
			}
		}

		public RenPyVariable() : base(RenPyStatementType.VARIABLE)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("$");
			tokens.Next();
			m_varName = tokens.Seek(new string [] {"=", "+", "-", "*", "/"}).Trim();

			switch (tokens.Next()) {
				case "=":
					m_assignment = ScriptableObject.CreateInstance<NormalAssignment>();
					break;
				case "+":
					m_assignment = ScriptableObject.CreateInstance<PlusAssignment>();
					tokens.Next();
					break;
				case "-":
					m_assignment = ScriptableObject.CreateInstance<MinusAssignment>();
					tokens.Next();
					break;
				case "*":
					m_assignment = ScriptableObject.CreateInstance<TimesAssignment>();
					tokens.Next();
					break;
				case "/":
					m_assignment = ScriptableObject.CreateInstance<DivideAssignment>();
					tokens.Next();
					break;
			}

			m_value = tokens.Seek("\n").Trim();
		}

		public override void Execute(RenPyState state)
		{
			m_assignment.Assign(state, m_varName, m_value);
		}

		public override string ToDebugString()
		{
			string str = "$" + m_varName;
			str += " " + m_assignment.GetOp();
			str += " \"" + m_value + "\"";
			return str;
		}
	}
}
