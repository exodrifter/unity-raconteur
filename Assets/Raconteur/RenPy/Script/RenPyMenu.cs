using System.Collections.Generic;

using DPek.Raconteur.RenPy.Parser;
using DPek.Raconteur.RenPy.State;

namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Ren'Py menu statement.
	/// </summary>
	public class RenPyMenu : RenPyStatement
	{
		public RenPyMenu(ref RenPyScanner tokens)
			: base(RenPyStatementType.MENU)
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

		public override string ToString()
		{
			string str = "menu";
			foreach (var item in GetChoices()) {
				str += " (\"" + item + "\")";
			}

			str += "\n" + base.ToString();
			return str;
		}
	}
}
