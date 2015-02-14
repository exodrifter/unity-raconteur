namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if the two arguments are equal
	/// </summary>
	public class OperatorLessThanOrEqual : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorLessThanOrEqual(string symbol) : base(symbol) {}

		/// <summary>
		/// Returns true if the left and right hand sides are equal
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
			bool result = Value.LessThanOrEqual(state, left, right);
			return new ValueBoolean(result);
		}
	}
}
