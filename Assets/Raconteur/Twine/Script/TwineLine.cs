using UnityEngine;
using System.Collections;

namespace DPek.Raconteur.Twine.Script
{
	/// <summary>
	/// Represents a part of a Twine passage.
	/// </summary>
	public abstract class TwineLine
	{
		/// <summary>
		/// Returns the contents to display for this line.
		/// </summary>
		/// <returns></returns>
		public abstract string Print();

		/// <summary>
		/// Returns a debug string for this line.
		/// </summary>
		/// <returns>
		/// The line as a debug string.
		/// </returns>
		protected abstract string ToDebugString();

		public sealed override string ToString()
		{
			return this.GetType().FullName + "\n" + ToDebugString();
		}
	}
}