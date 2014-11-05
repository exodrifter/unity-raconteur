using UnityEngine;

namespace DPek.Raconteur.Util
{
	public class ColorHexConverter
	{
		private ColorHexConverter()
		{
			// Nothing to do
		}

		// Converts a single hex character to an int value
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

		private static int FromHex(string str)
		{
			int totalVal = 0;
			for(int i = str.Length-1; i >= 0; --i) {
				totalVal *= 16;
				totalVal += FromHex(str[i]);
			}
			return totalVal;
		}

		public static Color FromRGB(string str)
		{
			if(str[0] == '#') {
				str = str.Substring(1);
			}
			float r, g, b = 0;

			// Figure out the size of each color in the #RGB format
			int size = str.Length / 3;
			float maxVal = Mathf.Pow(16,size);
			r = FromHex (str.Substring(0,size)) / maxVal;
			g = FromHex (str.Substring(size,size)) / maxVal;
			b = FromHex (str.Substring(size*2,size)) / maxVal;

			return new Color(r, g, b);
		}
	}
}