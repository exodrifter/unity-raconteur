using UnityEngine;
using System.Collections;

// TODO: Implment
namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// This represents a string containing python code.
	/// </summary>
	public class PyExpr : MonoBehaviour
	{
		public string str;
		public string filename;
		public int linenumber;

		public PyExpr (string str, string filename, int linenumber)
		{
			this.str = str;
			this.filename = filename;
			this.linenumber = linenumber;
		}
	}
}
