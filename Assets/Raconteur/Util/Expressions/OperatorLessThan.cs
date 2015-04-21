namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if the left hand side is less
	/// than the right hand side.
	/// </summary>
	public class OperatorLessThan : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorLessThan(string symbol) : base(symbol) { }

		/// <summary>
		/// Returns true if the left hand side is less than the right hand side.
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
			bool result = Value.LessThan(state, left, right);
			return new ValueBoolean(result);
		}
	}
}
