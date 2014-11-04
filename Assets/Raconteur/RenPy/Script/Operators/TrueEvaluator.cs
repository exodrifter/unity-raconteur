using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class TrueEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyState state, string variable,
		                              string value)
		{
			string current = state.GetVariable(variable);
			return current == "True";
		}
		
		public override string GetOp()
		{
			return ":";
		}
	}
}