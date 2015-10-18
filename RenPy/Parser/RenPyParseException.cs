namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// The exception that is thrown when there is an error parsing a Ren'Py
	/// script.
	/// </summary>
	class RenPyParseException : System.Exception
	{
		/// <summary>
		/// Creates a new RenPyParseException.
		/// </summary>
		/// <param name="file">
		/// The file that could not be parsed.
		/// </param>
		/// <param name="lineNumber">
		/// The line number of the file that could not be parsed.
		/// </param>
		/// <param name="message">
		/// The accompanying error message.
		/// </param>
		public RenPyParseException(string file, int lineNumber, string message)
			: base(file + ":" + lineNumber + " " + message)
		{
		}
	}
}