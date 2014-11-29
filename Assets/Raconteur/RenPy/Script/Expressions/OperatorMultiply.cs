using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents an operator that returns the argument it has.
	/// </summary>
	[System.Serializable]
	public class OperatorMultiply : Operator
	{
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
		public override Value Eval(RenPyState state, Value left, Value right)
		{
			return Value.Multiply(state, left, right);
		}
	}
}