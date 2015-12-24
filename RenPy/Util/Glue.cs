using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Contains methods to make reading translated python code easier.
	/// </summary>
	public static class Glue
	{
		/// <summary>
		/// Represents the in operator in Python for a character to an array of
		/// characters.
		/// 
		/// The method should return true if and only if the character can be
		/// found in the specified array.
		/// </summary>
		/// <param name="ch">The character to find.</param>
		/// <param name="arr">The array to search.</param>
		public static bool In (this char ch, params char[] arr)
		{
			foreach (char a in arr) {
				if (ch == a) return true;
			}
			return false;
		}

		/// <summary>
		/// Represents the in operator in Python for a character to a list of
		/// characters.
		/// 
		/// The method should return true if and only if the character can be
		/// found in the specified character list.
		/// </summary>
		/// <param name="ch">The character to find.</param>
		/// <param name="arr">The array to search.</param>
		public static bool In (this char ch, List<char> arr)
		{
			foreach (char a in arr) {
				if (ch == a) return true;
			}
			return false;
		}

		/// <summary>
		/// Represents the in operator in Python for a character to a string.
		/// 
		/// The method should return true if and only if the character can be
		/// found in the specified string.
		/// </summary>
		/// <param name="ch">The character to find.</param>
		/// <param name="arr">The string to search.</param>
		public static bool In (this char ch, string str)
		{
			foreach (char a in str) {
				if (ch == a) return true;
			}
			return false;
		}

		/// <summary>
		/// Represents the in operator in Python for a string to a string
		/// 
		/// The method should return true if and only if the string can be
		/// found in the specified string.
		/// </summary>
		/// <param name="str">The string to find.</param>
		/// <param name="other">The string to search.</param>
		public static bool In (this string str, string other)
		{
			return other.Contains (str);
		}

		/// <summary>
		/// Represents the in operator in Python for a string to an array of
		/// strings.
		/// 
		/// The method should return true if and only if the string can be
		/// found in the specified string array.
		/// </summary>
		/// <param name="str">The string to find.</param>
		/// <param name="other">The string array to search.</param>
		public static bool In (this string str, List<string> other)
		{
			return other.Contains (str);
		}

		/// <summary>
		/// Represents the slicing operator in Python for a string
		/// </summary>
		/// <param name="str">The string to slice.</param>
		/// <param name="start">The start index.</param>
		/// <param name="end">The end index.</param>
		public static string Slice (this string str, int? start, int? end)
		{
			start = start ?? 0;
			end = end ?? 0;
			end = str.Length + end;

			return str.Substring (start.Value, (end - start).Value);
		}

		// TODO: Implement count
		public static int Count (this string str, char ch)
		{
			int count = 0;
			foreach (char c in str)
			{
				if (c == ch) count++;
			}
			return count;
		}

		// TODO: Implement strip
		public static string Strip (this string str)
		{
			throw new NotImplementedException ();
		}

		public static List<T> Apply <T> (this List<T> list, Action<T> action) {
			list.ForEach (action);
			return list;
		}
	}

	/// <summary>
	/// A static class used to do regular expressions
	/// </summary>
	public static class re
	{
		public static int DOTALL = 0, S = 0;

		public static Regex compile (string regex, int? option = null)
		{
			// TODO: Manage options such as re.S, which forces the '.' to also
			// match newlines.
			return new Regex (regex);
		}

		// TODO: Actually escape the string
		public static string escape (string str)
		{
			throw new NotImplementedException ();
		}

		public static bool match (string regex, string str, RegexOptions options = RegexOptions.None)
		{
			return new Regex (regex, options).Matches (str).Count > 0;
		}

		/// <summary>
		/// Replaces each match from a regular expression with the result from
		/// a function.
		/// </summary>
		/// <param name="regex">The regex to use.</param>
		/// <param name="func">The function to use for replacements.</param>
		/// <param name="str">The string to replace.</param>
		public static string sub (string regex, Func<Match, string> func, string str)
		{
			return new Regex (regex).Replace (str, new MatchEvaluator(func));
		}

		/// <summary>
		/// Replaces each match from a regular expression with a string
		/// </summary>
		/// <param name="regex">The regex to use.</param>
		/// <param name="replacement">The string to use for replacements.</param>
		/// <param name="str">The string to replace.</param>
		public static string sub (string regex, string replacement, string str)
		{
			return new Regex (regex).Replace (str, replacement);
		}
	}
}
