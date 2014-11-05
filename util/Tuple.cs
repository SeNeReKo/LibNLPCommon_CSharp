using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class Tuple<T, U>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public Tuple(T t, U u)
		{
			Item1 = t;
			Item2 = u;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public T Item1
		{
			get;
			private set;
		}

		public U Item2
		{
			get;
			private set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public override bool Equals(object obj)
		{
			if (obj is Tuple<T, U>) {
				Tuple<T, U> t = (Tuple<T, U>)obj;
				return Util.Equals(t.Item1, Item1) && Util.Equals(t.Item2, Item2);
			} else {
				return false;
			}
		}

		public override int GetHashCode()
		{
			int n = 7;
			object o1 = (object)(Item1);
			if (o1 != null) n ^= o1.GetHashCode();
			object o2 = (object)(Item2);
			if (o2 != null) n ^= o2.GetHashCode();
			return n;
		}

		public override string ToString()
		{
			object o1 = (object)(Item1);
			object o2 = (object)(Item2);
			if (o1 == null) {
				if (o2 == null) {
					return "(null)";
				} else {
					return "(null) : " + o2;
				}
			} else {
				if (o2 == null) {
					return o1 + " : (null)";
				} else {
					return o1 + " : " + o2;
				}
			}
		}

	}

}
