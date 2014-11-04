using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	[System.Serializable]
	public abstract class Evaluator : ScriptableObject
	{
		public abstract bool Evaluate(RenPyState state, string variable,
		                              string value);
		public abstract string GetOp();
	}
}