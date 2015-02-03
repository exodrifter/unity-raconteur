using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that returns true if the two arguments are equal.
	/// </summary>
	public class OperatorEquals : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorEquals(string symbol) : base(symbol) {}

		/// <summary>
		/// Returns true if the left and right hand sides are equal.
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
			bool result = left.AsString(state) == right.AsString(state);
			return new ValueBoolean(result);
		}
	}
}
