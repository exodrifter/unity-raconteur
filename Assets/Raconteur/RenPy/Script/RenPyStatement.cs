namespace DPek.Raconteur.RenPy.Script
{
	/// <summary>
	/// Represents a statement in RenPy.
	/// </summary>
	public abstract class RenPyStatement
	{
		/// <summary>
		/// The type of this statement.
		/// </summary>
		private RenPyStatementType m_type;
		public RenPyStatementType Type
		{
			get {
				return m_type;
			}
		}

		/// <summary>
		/// Creates a new RenPyStatement of the specified type.
		/// </summary>
		/// <param name="type">
		/// The type of this RenPyStatement.
		/// </param>
		protected RenPyStatement(RenPyStatementType type)
		{
			this.m_type = type;
		}

		/// <summary>
		/// Executes the actions that this line takes.
		/// </summary>
		public abstract void Execute(RenPyDisplayState state);

		public override string ToString()
		{
			return this.GetType().FullName;
		}
	}
}
