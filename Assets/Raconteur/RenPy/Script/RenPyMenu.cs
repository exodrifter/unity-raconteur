using System.Collections.Generic;

using DPek.Raconteur.RenPy.State;
using DPek.Raconteur.Util.Parser;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py menu statement.
	/// </summary>
	public class RenPyMenu : RenPyStatement
	{
		/// <summary>
		/// Initializes this statement with the passed scanner.
		/// </summary>
		/// <param name="tokens">
		/// The scanner to use to initialize this statement.
		/// </param>
		public RenPyMenu(ref Scanner tokens) : base(RenPyStatementType.MENU)
		{
			tokens.Seek("menu");
			tokens.Seek("\n");
			tokens.Next();
		}

		public override void Execute(RenPyState state)
		{
			// Nothing to do
		}

		public List<string> GetChoices()
		{
			var choices = new List<string>();
			foreach(var block in NestedBlocks) {
				foreach(var statement in block.Statements) {
					if(statement is RenPyMenuChoice) {
						string choice = (statement as RenPyMenuChoice).Text;
						choices.Add(choice);
					}
				}
			}
			return choices;
		}

		public void PickChoice(RenPyState state, string choice)
		{
			List<RenPyBlock> blocks = null;
			foreach(var block in NestedBlocks) {
				foreach(var statement in block.Statements) {
					if(statement is RenPyMenuChoice) {
						var text = (statement as RenPyMenuChoice).Text;
						if(text == choice) {
							blocks = statement.NestedBlocks;
							break;
						}
					}
				}

				if(blocks != null) {
					break;
				}
			}
			state.Execution.PushStackFrame(blocks);
		}

		public override string ToDebugString()
		{
			string str = "menu";
			foreach (var item in GetChoices()) {
				str += " (\"" + item + "\")";
			}
			return str;
		}
	}
}
