using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	/// <summary>
	/// This class implements a read only or read-write set of key value pairs.
	/// </summary>
	public class SKVPSet : IEnumerable<SKVP>
	{

		public enum EnumPermutationMode
		{
			ReuseReturnedKVPSet,
			AlwaysReturnNewKVPSet,
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		public static readonly SKVPSet EMPTY = new SKVPSet(true);

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		List<SKVP> list;
		bool bReadOnly;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SKVPSet()
		{
			list = new List<SKVP>();
		}

		public SKVPSet(bool bReadOnly)
		{
			list = new List<SKVP>();
			this.bReadOnly = bReadOnly;
		}

		public SKVPSet(SKVPSet kvps)
		{
			list = new List<SKVP>();
			if (kvps != null) AddRange(kvps.ToArray());
		}

		public SKVPSet(IEnumerable<SKVP> kvps)
		{
			list = new List<SKVP>();
			if (kvps != null) AddRange(kvps);
		}

		public SKVPSet(params SKVP[] kvps)
		{
			list = new List<SKVP>();
			AddRange(kvps);
		}

		public SKVPSet(params string[] kvps)
		{
			list = new List<SKVP>();
			AddRange(kvps);
		}

		public SKVPSet(bool bReadOnly, params SKVP[] kvps)
		{
			list = new List<SKVP>();
			AddRange(kvps);
			this.bReadOnly = bReadOnly;
		}

		public SKVPSet(bool bReadOnly, params string[] kvps)
		{
			list = new List<SKVP>();
			AddRange(kvps);
			this.bReadOnly = bReadOnly;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool IsEmpty
		{
			get {
				return list.Count == 0;
			}
		}

		public bool IsReadOnly
		{
			get {
				return bReadOnly;
			}
			set {
				if (value) bReadOnly = true;
			}
		}

		public int Count
		{
			get {
				return list.Count;
			}
		}

		public SKVP this[string key]
		{
			get {
				for (int i = 0; i < this.Count; i++) {
					if (list[i].Key.Equals(key)) return list[i];
				}
				return null;
			}
		}

		public SKVP this[int index]
		{
			get {
				return list[index];
			}
		}

		public string[] Keys
		{
			get {
				return __CollectNames();
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void RemoveAllBut(params string[] keys)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			for (int i = list.Count - 1; i >= 0; i--) {
				SKVP kvp = list[i];
				bool bFound = false;
				foreach (string key in keys) {
					if (key.Equals(kvp.Key)) {
						bFound = true;
						break;
					}
				}
				if (bFound) continue;
				else list.RemoveAt(i);
			}
		}

		public void SortBy(params string[] keys)
		{
			List<SKVP> newList = new List<SKVP>();

			foreach (string key in keys) {
				for (int i = 0; i < list.Count; i++) {
					if (list[i].Key.Equals(key)) {
						newList.Add(list[i]);
						list.RemoveAt(i);
						break;
					}
				}
			}

			newList.AddRange(list);

			this.list = newList;
		}

		private string[] __CollectNames()
		{
			HashSet<string> tagNames = new HashSet<string>();
			foreach (SKVP kvp in list) {
				tagNames.Add(kvp.Key);
			}
			string[] rlist = tagNames.ToArray();
			Array.Sort(rlist);
			return rlist;
		}

		public void Add(SKVP kvp)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			Remove(kvp.Key);
			list.Add(kvp);
		}

		public void Add(string key, string value)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			SKVP kvp = new SKVP(key, value);
			Remove(key);
			list.Add(kvp);
		}

		public void Add(string kvp)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			SKVP kvp2 = kvp;

			Remove(kvp2.Key);
			list.Add(kvp2);
		}

		public void AddRange(IEnumerable<SKVP> kvps)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			foreach (SKVP kvp in kvps) {
				Remove(kvp.Key);
				list.Add(kvp);
			}
		}

		public void AddRange(IEnumerable<string> kvps)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			foreach (SKVP kvp in kvps) {
				Remove(kvp.Key);
				list.Add(kvp);
			}
		}

		public void AddRange(SKVPSet kvps)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			if (kvps != null) {
				foreach (SKVP kvp in kvps) {
					Remove(kvp.Key);
					list.Add(kvp);
				}
			}
		}

		public void AddRange(params SKVP[] kvps)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			if (kvps != null) {
				foreach (SKVP kvp in kvps) {
					Remove(kvp.Key);
					list.Add(kvp);
				}
			}
		}

		public void AddRange(params string[] kvps)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			if (kvps != null) {
				foreach (SKVP kvp in kvps) {
					Remove(kvp.Key);
					list.Add(kvp);
				}
			}
		}

		public bool Remove(string key)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			for (int i = 0; i < list.Count; i++) {
				if (list[i].Key.Equals(key)) {
					list.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		public bool RemoveAll(params string[] keys)
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			int n = list.Count;
			for (int i = list.Count - 1; i >= 0; i--) {
				foreach (string key in keys) {
					if (key.Equals(list[i].Key)) {
						list.RemoveAt(i);
						break;
					}
				}
			}

			return n != list.Count;
		}

		public bool Contains(string key)
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i].Key.Equals(key)) {
					return true;
				}
			}
			return false;
		}

		public SKVP Get(string key)
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i].Key.Equals(key)) return list[i];
			}
			return null;
		}

		public SKVP GetE(string key)
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i].Key.Equals(key)) return list[i];
			}
			throw new Exception("No such key: " + key);
		}

		public void Clear()
		{
			if (bReadOnly) throw new Exception("Set is read only!");

			list.Clear();
		}

		public IEnumerator<SKVP> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}

		public SKVP[] ToArray()
		{
			return list.ToArray();
		}

		/*
		public static implicit operator KVP[](string[] kvps)
		{
			KVP[] ret = new KVP[kvps.Length];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = kvps[i];
			}
			return ret;
		}
		*/

		public static implicit operator SKVP[](SKVPSet kvps)
		{
			SKVP[] ret = new SKVP[kvps.Count];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = kvps[i];
			}
			return ret;
		}

		/*
		public static implicit operator string[](KVP[] kvps)
		{
			string[] ret = new string[kvps.Length];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = kvps[i];
			}
			return ret;
		}
		*/

		public static implicit operator string[](SKVPSet kvps)
		{
			string[] ret = new string[kvps.Count];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = kvps[i];
			}
			return ret;
		}

		public static implicit operator string(SKVPSet kvps)
		{
			return kvps.ToString();
		}

		public static implicit operator SKVPSet(SKVP[] kvps)
		{
			return new SKVPSet(true, kvps);
		}

		public static implicit operator SKVPSet(string[] kvps)
		{
			return new SKVPSet(true, kvps);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder("[");
			for (int i = 0; i < list.Count; i++) {
				if (i > 0) sb.Append("; ");
				sb.Append(list[i].ToString());
			}
			sb.Append(']');
			return sb.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is IEnumerable<SKVP>) {
				int count = 0;
				foreach (SKVP kvp in (IEnumerable<SKVP>)obj) {
					SKVP existing = Get(kvp.Key);
					if ((existing == null) || (!Util.Equals(existing.Value, kvp.Value))) return false;
					count++;
				}
				if (count != list.Count) return false;
				return true;
			} else
			if (obj is SKVP[]) {
				int count = 0;
				foreach (SKVP kvp in (SKVP[])obj) {
					SKVP existing = Get(kvp.Key);
					if ((existing == null) || (!Util.Equals(existing.Value, kvp.Value))) return false;
					count++;
				}
				if (count != list.Count) return false;
				return true;
			} else
				return false;
		}

		public SKVPSet CloneObject()
		{
			SKVPSet ret = new SKVPSet();
			ret.list.AddRange(this.list);
			return ret;
		}

		public SKVPSet CloneObject(bool bReadOnly)
		{
			if (bReadOnly && this.bReadOnly) return this;

			SKVPSet ret = new SKVPSet(bReadOnly);
			ret.list.AddRange(this.list);
			return ret;
		}

		public SKVPSet ToReadOnly()
		{
			if (bReadOnly) return this;

			SKVPSet ret = new SKVPSet(true);
			ret.list.AddRange(this.list);
			return ret;
		}

		/// <summary>
		/// Get a subset of the first <code>count</code> elements.
		/// Of course calling this method only makes sense if the elements in the set
		/// are ordered.
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public SKVPSet SubSet(int count)
		{
			if (count > list.Count) count = list.Count;

			SKVPSet ret = new SKVPSet();
			for (int i = 0; i < count; i++) {
				SKVP kvp = list[i];
				ret.Add(kvp);
			}

			return ret;
		}

		/// <summary>
		/// Get a subset of the first <code>count</code> elements.
		/// Of course calling this method only makes sense if the elements in the set
		/// are ordered.
		/// </summary>
		/// <param name="bReadOnly"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public SKVPSet SubSet(bool bReadOnly, int count)
		{
			if (count > list.Count) count = list.Count;

			SKVPSet ret = new SKVPSet();
			for (int i = 0; i < count; i++) {
				SKVP kvp = list[i];
				ret.Add(kvp);
			}

			ret.bReadOnly = bReadOnly;
			return ret;
		}

		/// <summary>
		/// Get a subset of the elements from the set that match the specified key(s).
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public SKVPSet SubSet(params string[] keys)
		{
			SKVPSet ret = new SKVPSet();
			if (keys != null) {
				foreach (string key in keys) {
					SKVP kvp = Get(key);
					if (kvp != null) ret.Add(kvp);
				}
			}
			ret.bReadOnly = bReadOnly;
			return ret;
		}

		/// <summary>
		/// Get a subset of the elements from the set that match the specified key(s).
		/// </summary>
		/// <param name="bReadOnly"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		public SKVPSet SubSet(bool bReadOnly, params string[] keys)
		{
			SKVPSet ret = new SKVPSet();
			if (keys != null) {
				foreach (string key in keys) {
					SKVP kvp = Get(key);
					if (kvp != null) ret.Add(kvp);
				}
			}
			ret.bReadOnly = bReadOnly;
			return ret;
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpSetA"></param>
		/// <param name="kvpSetB"></param>
		/// <returns></returns>
		public static SKVPSet operator +(SKVPSet kvpSetA, SKVPSet kvpSetB)
		{
			return Unite(kvpSetA, kvpSetB);
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpSetA"></param>
		/// <param name="kvpB"></param>
		/// <returns></returns>
		public static SKVPSet operator +(SKVPSet kvpSetA, SKVP kvpB)
		{
			return Unite(kvpSetA, kvpB);
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpA"></param>
		/// <param name="kvpSetB"></param>
		/// <returns></returns>
		public static SKVPSet operator +(SKVP kvpA, SKVPSet kvpSetB)
		{
			return Unite(kvpA, kvpSetB);
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpSetA"></param>
		/// <param name="kvpSetB"></param>
		/// <returns></returns>
		public static SKVPSet Unite(SKVPSet kvpSetA, SKVPSet kvpSetB)
		{
			if (kvpSetA == null) {
				if (kvpSetB == null) {
					return null;
				} else {
					return kvpSetB.CloneObject();
				}
			} else {
				if (kvpSetB == null) {
					return kvpSetA.CloneObject();
				} else {
					SKVPSet newSet = kvpSetA.CloneObject(false);
					foreach (SKVP kvp in kvpSetB) {
						newSet.Add(kvp);
					}
					return newSet;
				}
			}
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpA"></param>
		/// <param name="kvpSetB"></param>
		/// <returns></returns>
		public static SKVPSet Unite(SKVP kvpA, SKVPSet kvpSetB)
		{
			if (kvpA == null) {
				if (kvpSetB == null) {
					return null;
				} else {
					return kvpSetB.CloneObject();
				}
			} else {
				if (kvpSetB == null) {
					return new SKVPSet(kvpA);
				} else {
					SKVPSet newSet = new SKVPSet(false, kvpA);
					foreach (SKVP kvp in kvpSetB) {
						newSet.Add(kvp);
					}
					return newSet;
				}
			}
		}

		/// <summary>
		/// Constructs a non-readonly set
		/// </summary>
		/// <param name="kvpSetA"></param>
		/// <param name="kvpB"></param>
		/// <returns></returns>
		public static SKVPSet Unite(SKVPSet kvpSetA, SKVP kvpB)
		{
			if (kvpSetA == null) {
				if (kvpB == null) {
					return null;
				} else {
					return new SKVPSet(kvpB);
				}
			} else {
				if (kvpB == null) {
					return kvpSetA.CloneObject();
				} else {
					SKVPSet newSet = kvpSetA.CloneObject(false);
					newSet.Add(kvpB);
					return newSet;
				}
			}
		}

		public string GetValue(string key)
		{
			for (int i = 0; i < list.Count; i++) {
				if (list[i].Key.Equals(key)) return list[i].Value;
			}
			return null;
		}

		public SKVPSet DeriveReadOnly()
		{
			if (bReadOnly) return this;
			else return CloneObject(true);
		}

		/// <summary>
		/// Check if each key-value-pair from this set has a corresponding match in the target set of key-value(s)-pairs.
		/// Please note that the value of the argument may contain multiple definitions!
		/// </summary>
		/// <param name="targetSet"></param>
		/// <returns>Returns <code>true</code> if all elements of this set match an element in the target set</returns>
		public bool MatchesAll(SKVP[] targetSet)
		{
			foreach (SKVP kvp in list) {
				bool b = false;
				foreach (SKVP other in targetSet) {
					if (kvp.Matches(other)) {
						b = true;
						break;
					}
				}
				if (!b) return false;
			}
			return true;
		}

		private static List<SKVP> __Parse(string kvpText)
		{
			if (!kvpText.StartsWith("[") || !kvpText.EndsWith("]")) throw new Exception("Syntax error: Not a valid KVP list!");

			List<SKVP> kvpList = new List<SKVP>();
			string[] elements = kvpText.Substring(1, kvpText.Length - 2).Split(';');
			foreach (string s1 in elements) {
				string s2 = s1.Trim();
				if (s2.Length == 0) {
					// list is empty!
					// throw new Exception("Syntax error: Not a valid KVP list!");
				} else {
					try {
						kvpList.Add(s2);
					} catch (Exception ee) {
						throw new Exception("Syntax error: \"" + s2 + "\" is not a valid KVP element!");
					}
				}
			}
			return kvpList;
		}

		public static SKVPSet Parse(string kvpText)
		{
			return new SKVPSet(__Parse(kvpText));
		}

		public IEnumerable<SKVPSet> GetPermutations(EnumPermutationMode mode)
		{
			if (list.Count == 0) yield break;

			// prepare set to return

			SKVPSet ret = null;
			if (mode == EnumPermutationMode.ReuseReturnedKVPSet) ret = new SKVPSet();

			// prepare permutations

			int maxI = list.Count - 1;
			int[] counters = new int[list.Count];
			string[] keys = new string[list.Count];
			string[][] values = new string[list.Count][];
			for (int i = 0; i < list.Count; i++) {
				keys[i] = list[i].Key;
				values[i] = list[i].Values;
			}

			// do permutate

			while (true) {
				// return data
				if (mode == EnumPermutationMode.AlwaysReturnNewKVPSet) ret = new SKVPSet();
				else ret.Clear();
				for (int i = 0; i <= maxI; i++) {
					ret.Add(keys[i], values[i][counters[i]]);
				}
				yield return ret;

				// increment ones position
				counters[maxI]++;

				// propagate carry
				bool bTerminate = false;
				for (int i = maxI; i >= 0; i--) {
					if (counters[i] >= values[i].Length) {
						counters[i] = 0;
						if (i == 0) {
							bTerminate = true;
							break;
						}
						counters[i - 1]++;
					} else {
						break;
					}
				}
				if (bTerminate) break;
			}
		}

	}

}
