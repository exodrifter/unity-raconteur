namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if the left hand side is
	/// greater than or equal to the right hand side.
	/// </summary>
	public class OperatorGreaterThanOrEqual : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorGreaterThanOrEqual(string symbol) : base(symbol) { }

		/// <summary>
		/// Returns true if the left hand side is greater than or equal to the
		/// right hand side.
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
			bool result = Value.GreaterThanOrEqual(state, left, right);
			return new ValueBoolean(result);
		}
	}
}
