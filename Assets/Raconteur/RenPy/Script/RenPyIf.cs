using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py if statement.
	/// </summary>
	public class RenPyIf : RenPyStatement
	{
		[SerializeField]
		private string m_varName;
		public string Name
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
		private Evaluator m_evaluator;
		public Evaluator Operator
		{
			get {
				return m_evaluator;
			}
		}

		public RenPyIf() : base(RenPyStatementType.IF)
		{
			// Nothing to do
		}
		
		public override void Parse(ref RenPyScanner tokens)
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
				m_evaluator = ScriptableObject.CreateInstance<TrueEvaluator>();
				return;
			} else if (eval == ">") {
				if (tokens.Peek() == "=") {
					m_evaluator = ScriptableObject.CreateInstance<GreaterEqualEvaluator>();
				} else {
					m_evaluator = ScriptableObject.CreateInstance<GreaterThanEvaluator>();
				}
				tokens.Next();
			} else if (eval == "<") {
				if (tokens.Peek() == "=") {
					m_evaluator = ScriptableObject.CreateInstance<LessEqualEvaluator>();
				} else {
					m_evaluator = ScriptableObject.CreateInstance<LessThanEvaluator>();
				}
				tokens.Next();
			} else if (eval == "=") {
				tokens.Next(); // Assume the next token is an "=" sign
				m_evaluator = ScriptableObject.CreateInstance<EqualEvaluator>();
			} else if (eval == "!") {
				tokens.Next(); // Assume the next token is an "=" sign
				m_evaluator = ScriptableObject.CreateInstance<NotEqualEvaluator>();
			} else if (eval == "is") {
				m_evaluator = ScriptableObject.CreateInstance<EqualEvaluator>();
				tokens.Next();
			}
			m_value = tokens.Next();
			tokens.Seek(":");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			// If evaluation succeeds, push back this block
			if (m_evaluator.Evaluate(state, m_varName, m_value)) {
				string msg = "if " + m_varName + m_evaluator.GetOp() + m_value;
				msg += " evaluated to true (" + m_varName + "=";
				msg += state.GetVariable(m_varName) + ")";
				Static.Log(msg);

				state.Execution.PushStackFrame(NestedBlocks);
			}
			// If evaluation fails, skip this block
			else {
				string msg = "if " + m_varName + m_evaluator.GetOp() + m_value;
				msg += " evaluated to false (" + m_varName + "=";
				msg += state.GetVariable(m_varName) + ")";
				Static.Log(msg);
			}
		}

		public override string ToDebugString()
		{
			string str = "if " + m_varName;
			str += " " + m_evaluator.GetOp();
			str += " " + m_value;
			return str;
		}
	}
}
