using UnityEngine;
using System.Collections;

namespace DPek.Raconteur.RenPy
{
	/// <summary>
	/// Helper class for running an arbitrary routine on an object.
	/// </summary>
	class RenPyCoroutine : MonoBehaviour
	{
		/// <summary>
		/// Stops all other coroutines that are running on this object and then runs
		/// the passed routine on this object.
		/// </summary>
		/// <returns>
		/// The coroutine.
		/// </returns>
		/// <param name="routine">
		/// The routine to run on this object.
		/// </param>
		public Coroutine BeginCoroutine(IEnumerator routine)
		{
			StopAllCoroutines();
			return StartCoroutine(routine);
		}
	}
}
