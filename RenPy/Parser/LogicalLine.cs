namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Represents a logical line in a file.
	/// </summary>
	public class LogicalLine
	{
		/// <summary>
		/// The full path to the file with the line in it.
		/// </summary>
		public string filename;

		/// <summary>
		/// The line number.
		/// </summary>
		public int number;

		/// <summary>
		/// The offset inside the file at which the line starts.
		/// </summary>
		public int start;

		/// <summary>
		/// The offset inside the file at which the line ends.
		/// </summary>
		public int end;

		/// <summary>
		/// The offset inside the lime where the line delimiter ends.
		/// </summary>
		public int end_delim;

		/// <summary>
		/// The text of the line.
		/// </summary>
		public string text;

		public LogicalLine (string filename, int number, int start)
		{
			this.filename = filename;
			this.number = number;
			this.start = start;
			this.end = start;
			this.end_delim = start;
			this.text = "";
		}

		public override string ToString ()
		{
			return string.Format ("<Line {0}:{1} {2}>", filename, number, text);
		}
	}
}
