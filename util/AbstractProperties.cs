using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public abstract class AbstractProperties
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

		public abstract string this[string key]
		{
			get;
			set;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public string GetAsStr(string key)
		{
			return this[key];
		}

		public string[] GetAsStrList(string key)
		{
			string s = this[key];
			if (s == null) return null;
			if (s.Length == 0) return new string[0];
			return s.Split('|');
		}

		public int GetAsInt32(string key, int defaultValue)
		{
			string v = this[key];
			if (v == null) return defaultValue;
			try {
				return Int32.Parse(v);
			} catch (Exception ee) {
				return defaultValue;
			}
		}

		public void Set(string key, string value)
		{
			this[key] = value;
		}

		public void Set(string key, string[] value)
		{
			if (value == null) {
				this[key] = null;
				return;
			}
			if (value.Length == 0) {
				this[key] = "";
				return;
			}

			StringBuilder sb = new StringBuilder(value[0]);
			for (int i = 1; i < value.Length; i++) {
				sb.Append('|');
				sb.Append(value[i]);
			}

			this[key] = sb.ToString();
		}

		public void Set(string key, int value)
		{
			this[key] = "" + value;
		}

	}

}
