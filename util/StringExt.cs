using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public static class StringExt
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

		public static string TryRemovePrefix(this string s, string somePrefix)
		{
			if (s == null) return null;
			if (s.StartsWith(somePrefix)) s = s.Substring(somePrefix.Length);
			return s;
		}

		public static string TryRemovePattern(this string s, string regexPattern)
		{
			Regex regex = new Regex(regexPattern);
			return TryRemovePattern(s, regex);
		}

		public static string TryRemovePattern(this string s, Regex regex)
		{
			Match m = regex.Match(s);
			if ((m == null) || !m.Success) return s;

			int index = m.Index;
			string text = m.Value;
			string s1 = s.Substring(0, index);
			string s2 = s.Substring(index + text.Length);
			return s1 + s2;
		}

		public static bool EndsWith(this string s, char c)
		{
			if (s == null) return false;
			if (s.Length == 0) return false;
			return s[s.Length - 1] == c;
		}

		public static bool StartsWith(this string s, char c)
		{
			if (s == null) return false;
			if (s.Length == 0) return false;
			return s[0] == c;
		}

		public static string RemoveLastChars(this string s, int count)
		{
			if (s == null) return null;
			if (count >= s.Length) return "";
			return s.Substring(0, s.Length - count);
		}

		public static string RemoveFirstChars(this string s, int count)
		{
			if (s == null) return null;
			if (count >= s.Length) return "";
			return s.Substring(count);
		}

		public static string RemoveFirstChar(this string s)
		{
			if (s == null) return null;
			if (s.Length <= 1) return "";
			return s.Substring(1);
		}

		public static string TryRemoveLastChar(this string s, char c)
		{
			if (s == null) return null;
			if (s.Length == 0) return "";
			if (s[s.Length - 1] == c) {
				return s.Substring(0, s.Length - 1);
			} else {
				return s;
			}
		}

		public static string TryRemoveFirstChar(this string s, char c)
		{
			if (s == null) return null;
			if (s.Length == 0) return "";
			if (s[0] == c) {
				return s.Substring(1);
			} else {
				return s;
			}
		}

		public static string RemoveLastChar(this string s)
		{
			if (s == null) return null;
			if (s.Length <= 1) return "";
			return s.Substring(0, s.Length - 1);
		}

		public static bool ContainsOnlyChars(this string s, string chars)
		{
			if (s == null) return false;
			if (s.Length == 0) return false;
			for (int i = 0; i < s.Length; i++) {
				char c = s[i];
				bool b = false;
				for (int j = 0; j < chars.Length; j++) {
					if (c == chars[j]) {
						b = true;
						break;
					}
				}
				if (!b) return false;
			}
			return true;
		}

		public static int IndexOf(this string haystack, string[] needles, out string needleFound)
		{
			needleFound = null;
			if (haystack == null) return -1;

			int j = Int32.MaxValue;
			int jl = 0;
			string s = null;
			for (int i = 0; i < needles.Length; i++) {
				string needle = needles[i];
				int n = haystack.IndexOf(needle);
				if (n < 0) continue;
				if (n == j) {
					// same index
					if (needle.Length > jl) {
						j = n;
						s = needle;
						jl = needle.Length;
					}
				} else
				if (n < j) {
					// earlier
					j = n;
					s = needle;
					jl = needle.Length;
				}
			}
			if (s != null) {
				needleFound = s;
				return j;
			} else {
				return -1;
			}
		}

		public static int IndexOf(this string haystack, string[] needles)
		{
			if (haystack == null) return -1;

			int j = Int32.MaxValue;
			int jl = 0;
			string s = null;
			for (int i = 0; i < needles.Length; i++) {
				string needle = needles[i];
				int n = haystack.IndexOf(needle);
				if (n < 0) continue;
				if (n == j) {
					// same index
					if (needle.Length > jl) {
						j = n;
						s = needle;
						jl = needle.Length;
					}
				} else
				if (n < j) {
					// earlier
					j = n;
					s = needle;
					jl = needle.Length;
				}
			}
			if (s != null) {
				return j;
			} else {
				return -1;
			}
		}

	}

}
