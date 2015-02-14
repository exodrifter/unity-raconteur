namespace DPek.Raconteur.Util
{
	public abstract class StoryState
	{
		public abstract string GetVariable(string name);

		public abstract void SetVariable(string name, string value);
	}
}
