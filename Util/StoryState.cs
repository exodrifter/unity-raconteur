namespace DPek.Raconteur.Util
{
	/// <summary>
	/// Interface that defines how to store and fetch variables.
	/// </summary>
	public abstract class StoryState
	{
		/// <summary>
		/// Gets the value of the specified variable.
		/// </summary>
		/// <returns>
		/// The value of the specified variable.
		/// </returns>
		/// <param name="name">
		/// The name of the variable to get the value of.
		/// </param>
		public abstract string GetVariable(string name);

		/// <summary>
		/// Sets the specified variable to the specified value.
		/// </summary>
		/// <param name="name">
		/// The name of the variable to set the value of.
		/// </param>
		/// <param name="value">
		/// The value to set the variable to.
		/// </param>
		public abstract void SetVariable(string name, string value);

		/// <summary>
		/// Deletes the specified variable.
		/// </summary>
		/// <param name="name">
		/// The name of the variable to delete.
		/// </param>
		public abstract void DeleteVariable(string name);
	}
}
