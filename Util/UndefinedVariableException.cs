using System;

namespace DPek.Raconteur.Util
{
	/// <summary>
	/// Thrown when an attempted access to an undefined variable has been made.
	/// </summary>
	public class UndefinedVariableException : Exception
	{
		public UndefinedVariableException(string variable)
			: base("Variable \"" + variable + "\" does not exist") { }

		public UndefinedVariableException(string variable, Exception inner)
			: base("Variable \"" + variable + "\" does not exist", inner) { }
	}
}
