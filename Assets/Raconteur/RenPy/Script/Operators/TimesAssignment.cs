using UnityEngine;
using System.Collections;

using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	public class TimesAssignment : Assignment
	{
		public override void Assign(RenPyState state, string varName, string value)
		{
			string current = state.GetVariable(varName);
			
			int iLeft;
			if (int.TryParse(current, out iLeft)) {
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (iLeft * iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (iLeft * fRight).ToString());
				}
			} else {
				float fLeft = float.Parse(current);
				int iRight;
				if (int.TryParse(value, out iRight)) {
					state.SetVariable(varName, (fLeft * iRight).ToString());
				} else {
					float fRight;
					fRight = float.Parse(value);
					state.SetVariable(varName, (fLeft * fRight).ToString());
				}
			}
		}
		
		public override string GetOp()
		{
			return "*=";
		}
	}
}
