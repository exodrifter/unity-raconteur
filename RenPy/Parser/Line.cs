using System.Collections.Generic;

namespace Exodrifter.Raconteur.RenPy
{
	public class Line
	{
		public string filename;
		public int number;
		public string text;
		public List<Line> block;

		public Line (string filename, int number, string text, List<Line> block = null)
		{
			this.filename = filename;
			this.number = number;
			this.text = text;
			this.block = block ?? new List<Line> ();
		}
	}
}
