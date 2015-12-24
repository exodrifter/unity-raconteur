using System;

namespace Exodrifter.Raconteur.RenPy
{
	public class ParseError : Exception
	{
		private string message;
		public override string Message
		{
			get { return message; }
		}

		public ParseError (string filename, int number, string msg, string line = null, int? pos = null, bool first = false)
		{
			message = string.Format ("File \"{0}\", line {1}: {2}", filename, number, msg);

			if (line != null)
			{
				var lines = line.Split('\n');

				if (lines.Length > 1)
				{
					char? open_string = null;
					int i = 0;

					while (i < lines[0].Length)
					{
						var c = lines[0][i];

						if (c == '\\') {
							i += 1;
						}
						else if (c == open_string) {
							open_string = null;
						}
						else if (open_string != null) {
							continue;
						}
						else if (c == '`' || c == '\'' || c == '\"') {
							open_string = c;
						}

						i += 1;
					}

					if (open_string != null) {
						message += string.Format ("\n(Perhaps you left out a {0} at the end of the first line.)", open_string);
					}
				}

				foreach (var l in lines)
				{
					message += "\n    " + l;

					if (pos != null) {
						if (pos.Value <= l.Length) {
							message += "\n    " + new string (' ', pos.Value) + "^";
							pos = null;
						}
						else {
							pos -= l.Length;
						}
					}

					if (first) {
						break;
					}
				}
			}
		}
	}
}
