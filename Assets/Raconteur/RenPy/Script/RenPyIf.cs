using DPek.Raconteur.RenPy.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	public class RenPyIf : RenPyLine
	{
		private string m_varName;
		public string Name
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

		private Evaluator m_evaluator;

		public RenPyIf(ref RenPyScanner tokens) : base(RenPyLineType.IF)
		{
			tokens.Seek("if");
			tokens.Next();

			// Get the variable name
			tokens.SkipWhitespace();
			m_varName = tokens.Next();
			tokens.SkipWhitespace();

			// Get the evaluation criterea
			string eval = tokens.Next();
			if (eval == ":") {
				// True if the variable is set to "True"
				m_evaluator = new TrueEvaluator();
				return;
			} else if (eval == ">") {
				if (tokens.Peek() == "=") {
					m_evaluator = new GreaterEqualEvaluator();
				} else {
					m_evaluator = new GreaterThanEvaluator();
				}
				tokens.Next();
			} else if (eval == "<") {
				if (tokens.Peek() == "=") {
					m_evaluator = new LessEqualEvaluator();
				} else {
					m_evaluator = new LessThanEvaluator();
				}
				tokens.Next();
			} else if (eval == "=") {
				tokens.Next(); // Assume the next token is an "=" sign
				m_evaluator = new EqualEvaluator();
			} else if (eval == "!") {
				tokens.Next(); // Assume the next token is an "=" sign
				m_evaluator = new NotEqualEvaluator();
			} else if (eval == "is") {
				m_evaluator = new EqualEvaluator();
				tokens.Next();
			}
			m_value = tokens.Next();
			tokens.Seek(":");
			tokens.Next();
		}

		public override void Execute(RenPyDisplayState display)
		{
			// If evaluation succeeds, go to the next line
			if (m_evaluator.Evaluate(display, m_varName, m_value)) {
				string msg = "if " + m_varName + m_evaluator.GetOp() + m_value;
				msg += " evaluated to true (" + m_varName + "=";
				msg += display.State.GetVariable(m_varName) + ")";
				Static.Log(msg);

				display.State.NextLine(display);
			}
			// If evaluation fails, skip the next line
			else {
				string msg = "if " + m_varName + m_evaluator.GetOp() + m_value;
				msg += " evaluated to false (" + m_varName + "=";
				msg += display.State.GetVariable(m_varName) + ")";
				Static.Log(msg);

				display.State.NextLine(display, false);
				display.State.NextLine(display);
			}
		}

		public override string ToString()
		{
			string str = "if " + m_varName;
			str += " " + m_evaluator.GetOp();
			str += " " + m_value;

			str += "\n" + base.ToString();
			return str;
		}
	}

	abstract class Evaluator
	{
		public abstract bool Evaluate(RenPyDisplayState display, string variable,
		                              string value);
		public abstract string GetOp();
	}

	class TrueEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);
			return current == "True";
		}

		public override string GetOp()
		{
			return ":";
		}
	}

	class GreaterThanEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return iLeft > iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return iLeft > fRight;
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return fLeft > iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return fLeft > fRight;
				}
			}
		}

		public override string GetOp()
		{
			return ">";
		}
	}

	class GreaterEqualEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return iLeft >= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return iLeft >= fRight;
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return fLeft >= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return fLeft >= fRight;
				}
			}
		}

		public override string GetOp()
		{
			return ">=";
		}
	}

	class LessThanEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return iLeft < iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return iLeft < fRight;
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return fLeft < iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return fLeft < fRight;
				}
			}
		}

		public override string GetOp()
		{
			return "<";
		}
	}

	class LessEqualEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);

			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return iLeft <= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return iLeft <= fRight;
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return fLeft <= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return fLeft <= fRight;
				}
			}
		}

		public override string GetOp()
		{
			return "<=";
		}
	}

	class EqualEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);
			return current == value;
		}

		public override string GetOp()
		{
			return "==";
		}
	}

	class NotEqualEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyDisplayState display, string variable,
		                              string value)
		{
			string current = display.State.GetVariable(variable);
			return current != value;
		}

		public override string GetOp()
		{
			return "!=";
		}
	}
}
