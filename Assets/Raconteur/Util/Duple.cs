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
			get
			{
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

		public Duple(A first, B second)
		{
			m_first = first;
			m_second = second;
		}

		public override bool Equals(System.Object obj)
		{
			Duple<A, B> other = obj as Duple<A, B>;
			if (other == null) {
				return false;
			}

			return (First == other.First) && (Second == other.Second);
		}

		public bool Equals(Duple<A, B> other)
		{
			if (other == null) {
				return false;
			}

			return (First == other.First) && (Second == other.Second);
		}
	}
}