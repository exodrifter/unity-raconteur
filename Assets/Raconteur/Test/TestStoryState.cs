using DPek.Raconteur.Util;

namespace DPek.Raconteur.Test
{
	/// <summary>
	/// A dummy test story state.
	/// </summary>
	public class TestStoryState : StoryState
	{
		public override string GetVariable(string name)
		{
			if (Static.Vars.ContainsKey(name))
			{
				return Static.Vars[name];
			}
			else
			{
				throw new UndefinedVariableException(name);
			}
		}

		public override void SetVariable(string name, string value)
		{
			Static.Vars[name] = value;
		}

		public override void DeleteVariable(string name)
		{
			Static.Vars.Remove(name);
		}
	}
}