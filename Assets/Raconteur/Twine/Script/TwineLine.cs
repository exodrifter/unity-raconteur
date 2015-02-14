using UnityEngine;
using System.Collections.Generic;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Represents a part of a Twine passage.
	/// </summary>
	public abstract class TwineLine
	{
		/// <summary>
		/// Returns the string to display for this line.
		/// </summary>
		/// <returns>
		/// The string to display for this line.
		/// </returns>
		public abstract string Print();

		/// <summary>
		/// Compiles the contents of this line.
		/// </summary>
		/// <returns></returns>
		public abstract List<TwineLine> Compile();

		/// <summary>
		/// Returns a debug string for this line.
		/// </summary>
		/// <returns>
		/// The line as a debug string.
		/// </returns>
		protected abstract string ToDebugString();

		/// <summary>
		/// Returns a string that represents this object for debugging purposes.
		/// </summary>
		/// <returns>
		/// A string that represents this object for debugging purposes.
		/// </returns>
		public sealed override string ToString()
		{
			return this.GetType().FullName + "\n" + ToDebugString();
		}
	}
}