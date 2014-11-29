using UnityEngine;
using System;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents an operator in an expression. An operator returns a result
	/// based on the left and right hand sides of an expression.
	/// </summary>
	[System.Serializable]
	public abstract class Operator : ScriptableObject
	{
		/// <summary>
		/// The symbol of the operator. This must be set for the operator to
		/// work properly in the serializer.
		/// </summary>
		[SerializeField]
		private string m_symbol;
		public string Symbol
		{
			get
			{
				return m_symbol;
			}
			set
			{
				m_symbol = value;
			}
		}

		/// <summary>
		/// Returns the result of the operator with the specified arguments.
		/// </summary>
		/// <param name="state">
		/// The state to evaluate this operator against.
		/// </param>
		/// <param name="left">
		/// The left hand side or value of the expression.
		/// </param>
		/// <param name="str">
		/// The right hand side or value of the expression.
		/// </param>
		/// <returns>
		/// The result of this operator with the specified arguments.
		/// </returns>
		public Value Evaluate(RenPyState state, object left, object right)
		{
			// Get the value of the left and right sides
			Value leftVal = null;
			if(left is Expression) {
				leftVal = (left as Expression).Evaluate(state);
			} else if (left is string) {
				if(Value.ParseNumber(left as string) != null) {
					leftVal = new ValueNumber(left as string);
				} else {
					leftVal = new ValueVariable(left as string);
				}
			} else if(left != null) {
				string msg = "Parameter must be an expression or a value";
				throw new ArgumentException(msg, "left");
			}
			
			Value rightVal = null;
			if(right is Expression) {
				rightVal = (right as Expression).Evaluate(state);
			} else if (right is string) {
				if(Value.ParseNumber(right as string) != null) {
					rightVal = new ValueNumber(right as string);
				} else {
					rightVal = new ValueVariable(right as string);
				}
			} else if(right != null) {
				string msg = "Parameter must be an expression or a value";
				throw new ArgumentException(msg, "right");
			}

			return Eval(state, leftVal, rightVal);
		}

		/// <summary>
		/// Returns the result of the operator with the specified arguments.
		/// </summary>
		/// <param name="state">
		/// The state to evaluate this operator against.
		/// </param>
		/// <param name="left">
		/// The left hand value of the expression.
		/// </param>
		/// <param name="str">
		/// The right hand value of the expression.
		/// </param>
		/// <returns>
		/// The result of this operator with the specified arguments.
		/// </returns>
		public abstract Value Eval(RenPyState state, Value left, Value right);

		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override sealed string ToString ()
		{
			return m_symbol;
		}
	}
}
