using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyVariable : RenPyStatement
	{
		private string m_varName;
		public string VarName
		{
			get {
				return m_varName;
			}
		}

		private string m_value;
		public string Value
		{
			get {
				return m_value;
			}
		}

		private Assignment m_assignment;

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
					m_assignment = new NormalAssignment();
					break;
				case "+":
					m_assignment = new PlusAssignment();
					tokens.Next();
					break;
				case "-":
					m_assignment = new MinusAssignment();
					tokens.Next();
					break;
				case "*":
					m_assignment = new TimesAssignment();
					tokens.Next();
					break;
				case "/":
					m_assignment = new DivideAssignment();
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

	abstract class Assignment
	{
		public abstract void Assign(RenPyState state, string varName, string value);
		public abstract string GetOp();
	}

	class NormalAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			state.SetVariable(varName, value);
		}

		public override string GetOp()
		{
			return "=";
		}
	}

	class PlusAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			string current = state.GetVariable(varName);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (iLeft + iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (iLeft + fRight).ToString());
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (fLeft + iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (fLeft + fRight).ToString());
				}
			}
		}

		public override string GetOp()
		{
			return "+=";
		}
	}

	class MinusAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			string current = state.GetVariable(varName);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (iLeft - iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (iLeft - fRight).ToString());
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (fLeft - iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (fLeft - fRight).ToString());
				}
			}
		}

		public override string GetOp()
		{
			return "-=";
		}
	}

	class TimesAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			string current = state.GetVariable(varName);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (iLeft * iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (iLeft * fRight).ToString());
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (fLeft * iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (fLeft * fRight).ToString());
				}
			}
		}

		public override string GetOp()
		{
			return "*=";
		}
	}

	class DivideAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			string current = state.GetVariable(varName);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (iLeft / iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (iLeft / fRight).ToString());
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (fLeft / iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (fLeft / fRight).ToString());
				}
			}
		}

		public override string GetOp()
		{
			return "/=";
		}
	}
}
