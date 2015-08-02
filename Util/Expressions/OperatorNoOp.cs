namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that performs no operation.
	/// </summary>
	public class OperatorNoOp : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorNoOp(string symbol) : base(symbol) { }

		/// <summary>
		/// Returns the left hand argument if it is non-null. If the left hand
		/// argument is null, this returns the right hand argument.
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
			return left != null ? left : right;
		}
	}
}
