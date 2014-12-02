using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents an operator that multiplies the right and left hand arguments
	/// and assigns that value to the left hand argument.
	/// </summary>
	public class OperatorAssignMultiply : Operator
	{
		/// <summary>
		/// Multiplies the value of the right and left hand arguments, assigns
		/// that value to the left hand argument, and returns the new value of
		/// the left hand argument.
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
			Value result = Value.Multiply(state, left, right);
			left.SetValue(state, result);
			return left.GetValue(state);
		}
	}
}
