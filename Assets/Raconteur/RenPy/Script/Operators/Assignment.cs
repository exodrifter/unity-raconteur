using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	[System.Serializable]
	public abstract class Assignment : ScriptableObject
	{
		public abstract void Assign(RenPyState state, string varName, string value);
		public abstract string GetOp();
	}
}
