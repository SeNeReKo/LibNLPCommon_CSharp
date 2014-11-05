using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public abstract class AbstractDictionary<K, V> : IDictionary<K, V>
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		protected Dictionary<K, V> innerDict;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public AbstractDictionary(params KeyValuePair<K, V>[] items)
		{
			this.innerDict = new Dictionary<K, V>();
			foreach (KeyValuePair<K, V> item in items) {
				innerDict.Add(item.Key, item.Value);
			}
		}

		public AbstractDictionary()
		{
			this.innerDict = new Dictionary<K, V>();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public virtual int Count
		{
			get {
				return innerDict.Count;
			}
		}

		public virtual ICollection<K> Keys
		{
			get {
				return innerDict.Keys;
			}
		}

		public virtual ICollection<V> Values
		{
			get {
				return innerDict.Values;
			}
		}

		public virtual V this[K key]
		{
			get {
				return innerDict[key];
			}
			set {
				innerDict[key] = value;
			}
		}

		public virtual bool IsReadOnly
		{
			get {
				return false;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public virtual void Add(K key, V value)
		{
			innerDict.Add(key, value);
		}

		public virtual void Add(KeyValuePair<K, V> kvp)
		{
			innerDict.Add(kvp.Key, kvp.Value);
		}

		public virtual void AddRange(IEnumerable<KeyValuePair<K, V>> items)
		{
			foreach (KeyValuePair<K, V> item in items) {
				Add(item);
			}
		}

		public virtual void AddRange(params KeyValuePair<K, V>[] items)
		{
			foreach (KeyValuePair<K, V> item in items) {
				Add(item);
			}
		}

		public virtual bool ContainsKey(K key)
		{
			return innerDict.ContainsKey(key);
		}

		public virtual bool Remove(K key)
		{
			return innerDict.Remove(key);
		}

		public virtual bool TryGetValue(K key, out V value)
		{
			return innerDict.TryGetValue(key, out value);
		}

		public virtual IEnumerator<KeyValuePair<K, V>> GetEnumerator()
		{
			return innerDict.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerDict.GetEnumerator();
		}

		public virtual bool Remove(KeyValuePair<K, V> kvp)
		{
			V value;
			if (innerDict.TryGetValue(kvp.Key, out value)) {
				if (kvp.Value == null) {
					if (value == null) {
						innerDict.Remove(kvp.Key);
						return true;
					} else {
						return false;
					}
				} else {
					if (kvp.Value.Equals(value)) {
						innerDict.Remove(kvp.Key);
						return true;
					} else {
						return false;
					}
				}
			}
			return false;
		}

		public virtual bool Contains(KeyValuePair<K, V> kvp)
		{
			return innerDict.Contains(kvp);
		}

		public virtual void CopyTo(KeyValuePair<K, V>[] array, int index)
		{
			List<KeyValuePair<K, V>> list = new List<KeyValuePair<K,V>>();
			foreach (KeyValuePair<K, V> kvp in innerDict) {
				list.Add(kvp);
			}
			list.CopyTo(array, index);
		}

		public virtual void Clear()
		{
			innerDict.Clear();
		}

		public override string ToString()
		{
			return innerDict.ToString();
		}

		public override bool Equals(object obj)
		{
			return innerDict.Equals(obj);
		}

		public override int GetHashCode()
		{
			return innerDict.GetHashCode();
		}

	}

}
