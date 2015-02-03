using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an operator that assigns the right hand argument to the left
	/// hand argument.
	/// </summary>
	public class OperatorAssign : Operator
	{
		/// <summary>
		/// Creates a new operator that is represented by the specified symbol.
		/// </summary>
		/// <param name="symbol">
		/// The symbol that represents this operator
		/// </param>
		public OperatorAssign(string symbol) : base(symbol) {}

		/// <summary>
		/// Assigns the right hand argument to the left hand argument and
		/// returns the new value of the left hand argument.
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
			left.SetValue(state, right);
			return left.GetValue(state);
		}
	}
}
