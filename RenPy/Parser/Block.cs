using System.Collections.Generic;

namespace Exodrifter.Raconteur.RenPy
{
	public class Block
	{
		public string filename;
		public int linenumber;
		public string text;
		public List<Block> block;

		public Block (string filename, int linenumber, string text, List<Block> block = null)
		{
			this.filename = filename;
			this.linenumber = linenumber;
			this.text = text;
			this.block = block ?? new List<Block> ();
		}
	}
}
