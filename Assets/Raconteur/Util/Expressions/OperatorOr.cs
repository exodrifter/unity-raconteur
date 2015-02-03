using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if either arguments are true.
	/// </summary>
	public class OperatorOr : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorOr(string symbol) : base(symbol) {}
		
		/// <summary>
		/// Returns true if either the left or right hand side is true.
		/// </summary>
		/// <param name="state">
		/// The state to evaluate this operator against.
		/// </param>
		/// <param name="left">
		/// The left hand argument.
		/// </param>
		/// <param name="right">
		/// The right hand argument.
		/// </param>
		public override Value Eval(RenPyState state, Value left, Value right)
		{
			bool leftVal;
			bool success = bool.TryParse(left.AsString(state), out leftVal);
			if(success && leftVal) {
				return new ValueBoolean(true);
			}

			bool rightVal;
			success = bool.TryParse(right.AsString(state), out rightVal);
			return new ValueBoolean(success && rightVal);
		}
	}
}
