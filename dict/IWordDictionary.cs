﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.dict
{

	public interface IWordDictionary
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

		IModelElement this[string alphanumericKey]
		{
			get;
			set;
		}

		IModelElement this[int numericKey]
		{
			get;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		int Put(string alphanumericKey, IModelElement element);
		bool TryPut(string alphanumericKey, IModelElement value, out int numericKey);

		IModelElement Lookup(string alphanumericKey);
		IModelElement Lookup(int numericKey);

		int GetNumericKey(string alphanumericKey);

	}

}
