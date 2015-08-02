using UnityEngine;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;
using DPek.Raconteur.Util.Expressions;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py else statement.
	/// </summary>
	public class RenPyElse : RenPyStatement
	{
		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyElse(ref Scanner tokens) : base(RenPyStatementType.ELSE)
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
