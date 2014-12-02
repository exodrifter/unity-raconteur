using UnityEngine;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py else statement.
	/// </summary>
	public class RenPyElse : RenPyStatement
	{
		public RenPyElse() : base(RenPyStatementType.ELSE)
		{
			// Nothing to do
		}

		public override void Parse(ref RenPyScanner tokens)
		{
			tokens.Seek("else");
			tokens.Next();
			tokens.Seek(":");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			// Check if evaluation is necessary
			var prev = state.Execution.GetPreviousStatement() as RenPyIf;
			if (prev == null) {
				var elif = state.Execution.GetPreviousStatement() as RenPyElif;
				if (elif == null) {
					var msg = "else expression has no preceding if statement";
					UnityEngine.Debug.LogError(msg);
					return;
				}
				else if (elif.WasSuccessful) {
					return;
				}
			}
			else if (prev.WasSuccessful) {
				return;
			}

			// If no previous statements evaluated as true, enter this block
			state.Execution.PushStackFrame(NestedBlocks);
		}

		public override string ToDebugString()
		{
			return "else:";
		}
	}
}
