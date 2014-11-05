using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.util
{

	public class CounterMap<T> : Dictionary<T, CounterInt32>
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

		public CounterMap(params T[] tokensToCount)
		{
			foreach (T t in tokensToCount) {
				Increment(t);
			}
		}

		public CounterMap()
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public KeyValuePair<T, int>[] Data
		{
			get {
				KeyValuePair<T, int>[] ret = new KeyValuePair<T, int>[Count];
				int i = 0;
				foreach (KeyValuePair<T, CounterInt32> kvp in this) {
					ret[i] = new KeyValuePair<T, int>(kvp.Key, kvp.Value.Value);
					i++;
				}
				return ret;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Set(T key, int value)
		{
			CounterInt32 c;
			if (this.TryGetValue(key, out c)) {
				c.Set(value);
			} else {
				this.Add(key, new CounterInt32(value));
			}
		}

		public void Increment(T key)
		{
			CounterInt32 c;
			if (this.TryGetValue(key, out c)) {
				c.Increment();
			} else {
				this.Add(key, new CounterInt32(1));
			}
		}

		public override string ToString()
		{
			return ToString(20);
		}

		public string ToString(int max)
		{
			StringBuilder sb = new StringBuilder();
			if (max < Count) max = Count;
			int i = 0;
			foreach (KeyValuePair<T, CounterInt32> kvp in this) {
				if (i > 0) {
					sb.Append(", ");
				}
				sb.Append(kvp.Key.ToString() + ":" + kvp.Value.ToString());
				i++;
			}
			if (max < Count) {
				sb.Append(", ...");
			}
			return sb.ToString();
		}

	}

}
