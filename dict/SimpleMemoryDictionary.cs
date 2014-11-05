using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.dict
{

	public class SimpleMemoryDictionary : IWordDictionary
	{

		public class Record : Dictionary<string, string>, IModelElement
		{

			public Record(int numericKey, string alphanumericKey, IDictionary<string, string> attributes)
				: base(attributes)
			{
				NumericKey = numericKey;
				AlphanumericKey = alphanumericKey;
			}

			public int NumericKey
			{
				get;
				private set;
			}

			public string AlphanumericKey
			{
				get;
				private set;
			}

		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Dictionary<string, Record> amap;
		Record[] nmap;
		int position;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public SimpleMemoryDictionary()
		{
			amap = new Dictionary<string, Record>();
			nmap = new Record[1024];
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public IModelElement this[string alphanumericKey]
		{
			get {
				Record r;
				if (amap.TryGetValue(alphanumericKey, out r)) return r;
				return null;
			}
			set {
				Record r;
				if (amap.TryGetValue(alphanumericKey, out r)) {
					if (value == null) {
						amap.Remove(alphanumericKey);
						nmap[r.NumericKey] = null;
					} else {
						r = new Record(r.NumericKey, alphanumericKey, value);
						amap.Remove(alphanumericKey);
						amap.Add(alphanumericKey, r);
						nmap[r.NumericKey] = r;
					}
				} else {
					r = new Record(position, alphanumericKey, value);
					amap.Add(alphanumericKey, r);
					__EnsureCapacity(position);
					nmap[position] = r;
					position++;
				}
			}
		}

		public IModelElement this[int numericKey]
		{
			get {
				if ((numericKey < 0) || (numericKey >= position)) return null;
				return nmap[position];
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __EnsureCapacity(int capacity)
		{
			int n = nmap.Length;
			while (n < capacity) n = n * 2;
			if (n == capacity) return;

			Record[] newNMap = new Record[n];
			nmap.CopyTo(newNMap, 0);
			nmap = newNMap;
		}

		////////////////////////////////////////////////////////////////

		public bool TryPut(string alphanumericKey, IModelElement value, out int numericKey)
		{
			Record r;
			if (amap.TryGetValue(alphanumericKey, out r)) {
				if (value == null) {
					amap.Remove(alphanumericKey);
					nmap[r.NumericKey] = null;
					numericKey = -1;
					return false;
				} else {
					numericKey = r.NumericKey;
					return false;
				}
			} else {
				r = new Record(position, alphanumericKey, value);
				amap.Add(alphanumericKey, r);
				__EnsureCapacity(position);
				nmap[position] = r;
				position++;
				numericKey = r.NumericKey;
				return true;
			}
		}

		public int Put(string alphanumericKey, IModelElement value)
		{
			Record r;
			if (amap.TryGetValue(alphanumericKey, out r)) {
				if (value == null) {
					amap.Remove(alphanumericKey);
					nmap[r.NumericKey] = null;
					return -1;
				} else {
					r = new Record(r.NumericKey, alphanumericKey, value);
					amap.Remove(alphanumericKey);
					amap.Add(alphanumericKey, r);
					nmap[r.NumericKey] = r;
				}
			} else {
				r = new Record(position, alphanumericKey, value);
				amap.Add(alphanumericKey, r);
				__EnsureCapacity(position);
				nmap[position] = r;
				position++;
			}
			return r.NumericKey;
		}

		public IModelElement Lookup(string alphanumericKey)
		{
			Record r;
			if (amap.TryGetValue(alphanumericKey, out r)) {
				return r;
			} else {
				return null;
			}
		}

		public IModelElement Lookup(int numericKey)
		{
			if ((numericKey < 0) || (numericKey >= position)) return null;
			return nmap[position];
		}

		public int GetNumericKey(string alphanumericKey)
		{
			Record r;
			if (amap.TryGetValue(alphanumericKey, out r)) {
				return r.NumericKey;
			} else {
				return -1;
			}
		}

	}

}
