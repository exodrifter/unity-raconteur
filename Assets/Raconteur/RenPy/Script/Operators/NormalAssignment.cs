using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class NormalAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			state.SetVariable(varName, value);
		}
		
		public override string GetOp()
		{
			return "=";
		}
	}
}
