using System;
using System.Text.RegularExpressions;

namespace Exodrifter.Raconteur.RenPy.Util
{
	/// <summary>
	/// A static class used to do regular expressions as if the Python re
	/// module existed.
	/// </summary>
	public static class PythonRegex
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

		public static bool match (string regex, string str)
		{
			return new Regex (regex).Matches (str).Count > 0;
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