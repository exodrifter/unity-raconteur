using UnityEngine;
using System.Collections;

namespace Exodrifter.Raconteur.RenPy.Util
{
	public class Tuple<T1, T2>
	{
		public T1 a;
		public T2 b;

		public Tuple (T1 a, T2 b)
		{
			this.a = a;
			this.b = b;
		}
	}

	public class Tuple<T1, T2, T3>
	{
		public T1 a;
		public T2 b;
		public T3 c;

		public Tuple (T1 a, T2 b, T3 c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
		}
	}
}
