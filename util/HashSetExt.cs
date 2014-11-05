﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public static class HashSetExt
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

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static string ToSortedString<T>(this HashSet<T> data)
		{
			string[] values = new string[data.Count];

			int i = 0;
			foreach (T s in data) {
				values[i] = s.ToString();
				i++;
			}

			Array.Sort(values);

			StringBuilder sb = new StringBuilder("[");
			bool b = false;
			foreach (string s in values) {
				if (b) {
					sb.Append(", ");
				} else {
					b = true;
				}

				sb.Append(s);
			}
			sb.Append("]");

			return sb.ToString();
		}

	}

}
