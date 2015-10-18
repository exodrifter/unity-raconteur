namespace Exodrifter.Raconteur.RenPy
{
	/// <summary>
	/// Represents a statement in RenPy.
	/// </summary>
	public abstract class RenPyStatement
	{
		/// <summary>
		/// Executes this statement.
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// Returns the statement as a debug string.
		/// </summary>
		/// <returns>
		/// The statement as a debug string.
		/// </returns>
		public abstract string ToDebugString();

		public sealed override string ToString()
		{
			return ToDebugString() + "\n" + GetType().FullName;
		}
	}
}
