namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if both arguments are true.
	/// </summary>
	public class OperatorAnd : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorAnd(string symbol) : base(symbol) { }

		/// <summary>
		/// Returns true if the left and right hand sides are both true.
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
		public override Value Eval(StoryState state, Value left, Value right)
		{
			bool leftVal = false, rightVal = false;
			bool success = bool.TryParse(left.AsString(state), out leftVal);
			success = success && bool.TryParse(right.AsString(state), out rightVal);
			return new ValueBoolean(success && leftVal && rightVal);
		}
	}
}
