using UnityEngine;
using System;
using System.Collections;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// Represents an expression, which is an operator and one or two arguments.
	/// </summary>
	public sealed class Expression
	{
		/// <summary>
		/// The operator for this expression.
		/// </summary>
		private Operator m_op;
		public Operator Operator
		{
			get { return m_op; }
			private set { m_op = value; }
		}

		/// <summary>
		/// The left hand side of this expression.
		/// </summary>
		private string m_leftStr;
		private Expression m_leftExpression;
		public object Left
		{
			get
			{
				if (m_leftExpression != null)
				{
					return m_leftExpression;
				}
				return m_leftStr;
			}
			private set
			{
				if (value == null)
				{
					m_leftStr = null;
					m_leftExpression = null;
					return;
				}
				if (value is string)
				{
					m_leftStr = value as string;
					m_leftExpression = null;
					return;
				}
				if (value is Expression)
				{
					m_leftStr = null;
					m_leftExpression = value as Expression;
					return;
				}
				string msg = "Value must be a string or an expression";
				throw new InvalidOperationException(msg);
			}
		}

		/// <summary>
		/// The right hand side of this expression.
		/// </summary>
		private string m_rightStr;
		private Expression m_rightExpression;
		public object Right
		{
			get
			{
				if (m_rightExpression != null)
				{
					return m_rightExpression;
				}
				return m_rightStr;
			}
			private set
			{
				if (value == null)
				{
					m_rightStr = null;
					m_rightExpression = null;
					return;
				}
				if (value is string)
				{
					m_leftStr = value as string;
					m_leftExpression = null;
					return;
				}
				if (value is Expression)
				{
					m_rightStr = null;
					m_rightExpression = value as Expression;
					return;
				}
				string msg = "Value must be a string or an expression";
				throw new InvalidOperationException(msg);
			}
		}

		/// <summary>
		/// Returns the value of this expression as evaluated by the operator
		/// when given the left and right hand sides of this expression.
		/// </summary>
		/// <param name="state">
		/// The state to evaluate this expression with.
		/// </param>
		/// <returns>
		/// The value of this expression.
		/// </returns>
		public Value Evaluate(StoryState state)
		{
			return m_op.Evaluate(state, Left, Right);
		}

		public Expression(Operator op, object left, object right)
		{
			m_op = op;
			Left = left;
			Right = right;
		}

		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public override string ToString()
		{
			if (m_op is OperatorNoOp)
			{
				if (Left != null)
				{
					return Left.ToString();
				}
				if (Right != null)
				{
					return Right.ToString();
				}
				return "()";
			}
			else
			{
				return "(" + m_op + " " + Left + " " + Right + ")";
			}
		}
	}
}
