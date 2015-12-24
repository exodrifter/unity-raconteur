using System;

namespace Exodrifter.Raconteur.RenPy.Util
{
	/// <summary>
	/// A collection of general Python functions.
	/// </summary>
	public static class Python
	{
		/// <summary>
		/// Convert an integer number (of any size) to a lowercase hexadecimal
		/// string prefixed with “0x”.
		/// </summary>
		/// <param name="i">The integer to convert.</param>
		public static string hex (int i) {
			return string.Format ("0x{0:X}", i);
		}

		/// <summary>
		/// Converts a string of length one into an integer.
		/// </summary>
		/// <param name="str">The string to convert.</param>
		public static int ord (string str) {
			if (str.Length > 1)
				throw new InvalidOperationException ("Invalid size");
			return (int) str[0];
		}
	}
}
