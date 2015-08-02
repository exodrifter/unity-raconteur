using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DPek.Raconteur.Util.Parser
{
	class ParseException : Exception
	{
		public ParseException() : base() {}

		public ParseException(string msg) : base(msg) { }
	}
}
