using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents an operator that returns true if the two arguments are equal
	/// </summary>
	public class OperatorGreaterThanOrEqual : Operator
	{
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
		public override Value Eval(RenPyState state, Value left, Value right)
		{
			bool result = Value.GreaterThanOrEqual(state, left, right);
			return new ValueString(result.ToString());
		}
	}
}
