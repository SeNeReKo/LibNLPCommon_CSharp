﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.util
{

	public class FlagSet
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Dictionary<string, bool> flags;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public FlagSet()
		{
			flags = new Dictionary<string, bool>();
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public bool this[string flagName]
		{
			get {
				bool b;
				if (flags.TryGetValue(flagName, out b)) return b;
				return false;
			}
			set {
				flags[flagName] = value;
			}
		}

		public string[] FlagNames
		{
			get {
				string[] ret = (new List<string>(flags.Keys)).ToArray();
				Array.Sort(ret);
				return ret;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Clear()
		{
			flags.Clear();
		}

	}

}
