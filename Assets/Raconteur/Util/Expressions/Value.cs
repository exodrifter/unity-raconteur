using UnityEngine;
using System;

namespace DPek.Raconteur.Util.Expressions
{
	/// <summary>
	/// A value represents an item in an expression. It can be a number, a
	/// variable, or a string.
	/// </summary>
	public abstract class Value
	{
		public abstract Value GetValue(StoryState state);

		public abstract object GetRawValue(StoryState state);

		public abstract void SetValue(StoryState state, Value value);

		public abstract string AsString(StoryState state);

		#region Arithmetic

		public static Value Add(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);

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

		public static Value Minus(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static Value Multiply(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static Value Divide(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static Value Mod(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static bool GreaterThan(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static bool GreaterThanOrEqual(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static bool LessThan(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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

		public static bool LessThanOrEqual(StoryState state, Value left, Value right) 
		{
			object leftNum = left.GetRawValue(state);
			object rightNum = right.GetRawValue(state);
			
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
