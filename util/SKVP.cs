using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	/// <summary>
	/// This class implements a read only key value pair.
	/// </summary>
	public class SKVP
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

		public SKVP(string key, string value)
		{
			if ((key == null) || (key.Length == 0) || (key.IndexOf(':') >= 0)) throw new Exception("Invalid key specified!");
			if ((value == null) || (value.Length == 0) || (value.IndexOf(':') >= 0)) throw new Exception("Invalid value specified!");

			this.Key = key;
			this.Value = value;
		}

		public SKVP(string key, params string[] values)
		{
			if ((key == null) || (key.Length == 0) || (key.IndexOf(':') >= 0)) throw new Exception("Invalid key specified!");
			if ((values == null) || (values.Length == 0)) throw new Exception("Invalid values specified!");

			this.Key = key;

			if (values.Length == 1) {
				if ((values[0] == null) || (values[0].Trim().Length != values[0].Length) || (values[0].Length == 0) || (values[0].IndexOf(':') >= 0))
					throw new Exception("Invalid value specified!");
				this.Value = values[0];
			} else {
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < values.Length; i++) {
					if (i > 0) sb.Append(',');
					if ((values[i] == null) || (values[i].Trim().Length != values[i].Length) || (values[i].Length == 0) || (values[i].IndexOf(':') >= 0))
						throw new Exception("Invalid values specified!");
					sb.Append(values[i]);
				}
				this.Value = sb.ToString();
			}
		}

		public SKVP(string key, IEnumerable<string> values)
			: this(key, values.ToArray())
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public int CountValues
		{
			get {
				int pos = Value.IndexOf(',');
				if (pos >= 0) {
					string[] elements = Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					return elements.Length;
				} else {
					return 1;
				}
			}
		}

		public string[] Values
		{
			get {
				int pos = Value.IndexOf(',');
				if (pos >= 0) {
					string[] elements = Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					return elements;
				} else {
					return new string[] { Value };
				}
			}
		}

		public string Key
		{
			get;
			private set;
		}

		public string Value
		{
			get;
			private set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public bool ContainsValue(string s)
		{
			foreach (string value in Values) {
				if (value.Equals(s)) return true;
			}
			return false;
		}

		public override string ToString()
		{
			return Key + ":" + Value;
		}

		public static implicit operator SKVP(string s)
		{
			string[] elements = s.Split(':');
			if (elements.Length != 2) throw new Exception("Invalid string: " + s);
			return new SKVP(elements[0], elements[1]);
		}

		public static implicit operator string(SKVP kvp)
		{
			if (kvp == null) return null;
			return kvp.ToString();
		}

		public static SKVP[] Parse(string[] elements)
		{
			SKVP[] ret = new SKVP[elements.Length];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = elements[i];
			}
			return ret;
		}

		/// <summary>
		/// Performs an exact match.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is SKVP) {
				SKVP kvp = (SKVP)obj;
				return Util.Equals(kvp.Key, Key) && Util.Equals(kvp.Value, Value);

			} else {
				return false;
			}
		}

		/// <summary>
		/// Check if this key-value-pair matches against the specified key-value(s)-pair.
		/// Please note that the value of the argument may contain multiple definitions!
		/// </summary>
		/// <param name="kvp"></param>
		/// <returns></returns>
		public bool Matches(SKVP kvp)
		{
			if (Util.Equals(kvp.Key, Key)) {
				int pos = kvp.Value.IndexOf(',');
				if (pos >= 0) {
					string[] elements = kvp.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string s in elements) {
						if (Util.Equals(Value, s)) return true;
					}
				} else {
					return Util.Equals(Value, kvp.Value);
				}
			}
			return false;
		}

		public IEnumerable<SKVP> UnwindToIndividualKVPs()
		{
			foreach (string value in Values) {
				yield return new SKVP(Key, value);
			}
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

	}

}
