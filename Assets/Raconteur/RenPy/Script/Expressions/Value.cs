using UnityEngine;
using System;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// A value represents an item in an expression. It can be a number, a
	/// variable, or a string.
	/// </summary>
	public abstract class Value
	{
		public abstract object GetValue(RenPyState state);
		
		public abstract void SetValue(RenPyState state, string value);

		#region Arithmetic

		public static Value Add(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);

			if(leftNum is int)
			{
				if(rightNum is int)
				{
					var num = (leftNum as int?) + (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as int?) + (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as int?) + (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					var num = (leftNum as float?) + (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as float?) + (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as float?) + (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					var num = (leftNum as double?) + (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as double?) + (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as double?) + (rightNum as double?);
					return new ValueNumber(num);
				}
			}

			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static Value Minus(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					var num = (leftNum as int?) - (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as int?) - (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as int?) - (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					var num = (leftNum as float?) - (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as float?) - (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as float?) - (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					var num = (leftNum as double?) - (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as double?) - (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as double?) - (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}

		public static Value Multiply(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					var num = (leftNum as int?) * (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as int?) * (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as int?) * (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					var num = (leftNum as float?) * (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as float?) * (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as float?) * (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					var num = (leftNum as double?) * (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as double?) * (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as double?) * (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static Value Divide(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					var num = (leftNum as int?) / (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as int?) / (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as int?) / (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					var num = (leftNum as float?) / (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as float?) / (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as float?) / (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					var num = (leftNum as double?) / (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as double?) / (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as double?) / (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static Value Mod(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					var num = (leftNum as int?) % (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as int?) % (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as int?) % (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					var num = (leftNum as float?) % (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as float?) % (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as float?) % (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					var num = (leftNum as double?) % (rightNum as int?);
					return new ValueNumber(num);
				}
				if(rightNum is float)
				{
					var num = (leftNum as double?) % (rightNum as float?);
					return new ValueNumber(num);
				}
				if(rightNum is double)
				{
					var num = (leftNum as double?) % (rightNum as double?);
					return new ValueNumber(num);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}

		#endregion

		#region Comparison

		public static bool GreaterThan(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					return (leftNum as int?) > (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as int?) > (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as int?) > (rightNum as double?);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					return (leftNum as float?) > (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as float?) > (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as float?) > (rightNum as double?);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					return (leftNum as double?) > (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as double?) > (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as double?) > (rightNum as double?);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static bool GreaterThanOrEqual(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					return (leftNum as int?) >= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as int?) >= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as int?) >= (rightNum as double?);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					return (leftNum as float?) >= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as float?) >= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as float?) >= (rightNum as double?);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					return (leftNum as double?) >= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as double?) >= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as double?) >= (rightNum as double?);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static bool LessThan(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					return (leftNum as int?) < (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as int?) < (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as int?) < (rightNum as double?);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					return (leftNum as float?) < (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as float?) < (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as float?) < (rightNum as double?);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					return (leftNum as double?) < (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as double?) < (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as double?) < (rightNum as double?);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}
		
		public static bool LessThanOrEqual(RenPyState state, Value left, Value right) 
		{
			object leftNum = left.GetValue(state);
			object rightNum = right.GetValue(state);
			
			if(leftNum is int)
			{
				if(rightNum is int)
				{
					return (leftNum as int?) <= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as int?) <= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as int?) <= (rightNum as double?);
				}
			}
			if(leftNum is float)
			{
				if(rightNum is int)
				{
					return (leftNum as float?) <= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as float?) <= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as float?) <= (rightNum as double?);
				}
			}
			if(leftNum is double)
			{
				if(rightNum is int)
				{
					return (leftNum as double?) <= (rightNum as int?);
				}
				if(rightNum is float)
				{
					return (leftNum as double?) <= (rightNum as float?);
				}
				if(rightNum is double)
				{
					return (leftNum as double?) <= (rightNum as double?);
				}
			}
			
			throw new ArgumentException("Arguments are not numbers");
		}

		#endregion

		/// <summary>
		/// Attempts to parse the passed string as a number. If it cannot, it
		/// returns null instead.
		/// </summary>
		/// <param name="str">
		/// The string to convert into a number.
		/// </param>
		/// <returns>
		/// The number represented by the string, or null if the string could
		/// not be converted into a number.
		/// </returns>
		public static object ParseNumber(string str)
		{
			// Try to parse the value as an int
			int intResult;
			if (int.TryParse(str, out intResult)) {
				return intResult;
			}

			// Try to parse the value as an float
			float floatResult;
			if (float.TryParse(str, out floatResult)) {
				return floatResult;
			}

			// Try to parse the value as a double
			double doubleResult;
			if (double.TryParse(str, out doubleResult)) {
				return doubleResult;
			}

			return null;
		}
	}
}
