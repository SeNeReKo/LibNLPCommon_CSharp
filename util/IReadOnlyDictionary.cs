﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public interface IReadOnlyDictionary<TKey, TValue> : IEnumerable
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

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		ICollection<TKey> Keys
		{
			get;
		}
		
		ICollection<TValue> Values
		{
			get;
		}

		int Count
		{
			get;
		}
		
		TValue this[TKey key]
		{
			get;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		bool ContainsKey(TKey key);

		bool TryGetValue(TKey key, out TValue value);

		bool Contains(KeyValuePair<TKey, TValue> item);

		void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex);

		IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

	}

}
