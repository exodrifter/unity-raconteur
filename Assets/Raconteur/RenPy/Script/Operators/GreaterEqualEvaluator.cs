using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class GreaterEqualEvaluator : Evaluator
	{
		public override bool Evaluate(RenPyState state, string variable,
		                              string value)
		{
			string current = state.GetVariable(variable);
			
			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return iLeft >= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return iLeft >= fRight;
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					return fLeft >= iRight;
				} else {
					float fRight;
					fRight = float.Parse(value);
					return fLeft >= fRight;
				}
			}
		}
		
		public override string GetOp()
		{
			return ">=";
		}
	}
}
