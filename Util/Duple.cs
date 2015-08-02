namespace DPek.Raconteur.Util
{
	/// <summary>
	/// A utility class for storing two objects as one object.
	/// </summary>
	/// <typeparam name="A">
	/// The type of the first object.
	/// </typeparam>
	/// <typeparam name="B">
	/// The type of the second object.
	/// </typeparam>
	public class Duple<A, B>
	{
		/// <summary>
		/// The first object.
		/// </summary>
		private A m_first;
		public A First
		{
			get {
				return m_first;
			}
			set {
				m_first = value;
			}
		}

		/// <summary>
		/// The second object.
		/// </summary>
		private B m_second;
		public B Second
		{
			get {
				return m_second;
			}
			set {
				m_second = value;
			}
		}

		/// <summary>
		/// Creates a new duple.
		/// </summary>
		/// <param name="first">
		/// The first object to store
		/// </param>
		/// <param name="second">
		/// The second object to store
		/// </param>
		public Duple(A first, B second)
		{
			m_first = first;
			m_second = second;
		}
	}
}