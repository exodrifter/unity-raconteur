using UnityEngine;

namespace DPek.Raconteur.Util
{
	/// <summary>
	/// A utility for converting hex strings such as #F93 to colors.
	/// </summary>
	public class ColorHexConverter
	{
		private ColorHexConverter()
		{
			// Nothing to do
		}

		/// <summary>
		/// Converts a single hex character to the corresponding int value.
		/// </summary>
		/// <returns>
		/// The number value of the hex character.
		/// </returns>
		/// <param name="ch">
		/// The character to get the hex value of.
		/// </param>
		private static int FromHex(char c)
		{
			c = char.ToLower(c);

			// Check if the character is a letter
			if(96 < c && c < 103) {
				return c - 86;
			}
			// Check if the character is a number
			else if (47 < c && c < 58) {
				return c - 48;
			}

			// Invalid character
			var msg = "\'" + c + "\' is not a hex character.";
			throw new System.ArgumentException(msg);
		}

		/// <summary>
		/// Converts a hex string to a number.
		/// </summary>
		/// <returns>
		/// The number value of the hex string.
		/// </returns>
		/// <param name="str">
		/// The string to get the hex value of.
		/// </param>
		private static int FromHex(string str)
		{
			int totalVal = 0;
			for(int i = str.Length-1; i >= 0; --i) {
				totalVal *= 16;
				totalVal += FromHex(str[i]);
			}
			return totalVal;
		}

		/// <summary>
		/// Parses a hex string in the format #RGB or RGB.
		/// 
		/// The hex string (when considered without the '#' character) may be
		/// any multiple of 3. For example, if the hex string is #FF9930, the
		/// format is assumed to be #RRGGBB or RRGGBB.
		/// </summary>
		/// <returns>
		/// The color as defined by the hex string.
		/// </returns>
		/// <param name="str">
		/// The hex string that defines the color.
		/// </param>
		public static Color FromRGB(string str)
		{
			if(str[0] == '#') {
				str = str.Substring(1);
			}

			// Figure out the size of each color in the #RGB format
			int size = str.Length / 3;
			float maxVal = Mathf.Pow(16, size);

			// Calculate RGB values
			float r, g, b = 0;
			r = FromHex(str.Substring(0, size)) / maxVal;
			g = FromHex(str.Substring(size, size)) / maxVal;
			b = FromHex(str.Substring(size*2, size)) / maxVal;

			return new Color(r, g, b);
		}
	}
}