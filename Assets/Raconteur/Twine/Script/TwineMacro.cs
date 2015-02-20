using DPek.Raconteur.Twine.State;
using UnityEngine;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Represents a part of a Twine passage that isn't used directly.
	/// </summary>
	public abstract class TwineMacro : TwineLine
	{
		/// <summary>
		/// Macros cannot be printed directly; they must be compiled.
		/// </summary>
		/// <returns>
		/// A null string.
		/// </returns>
		public override sealed string Print() { return null; }
	}
}