﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.bgtask
{

	public class Argument
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

		public Argument(ArgumentDescription argDescr, string value)
		{
			this.ArgumentDescription = argDescr;
			this.Value = value;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string ID
		{
			get {
				return ArgumentDescription.ID;
			}
		}

		public string Value
		{
			get;
			private set;
		}

		public ArgumentDescription ArgumentDescription
		{
			get;
			private set;
		}

		public bool IsValid
		{
			get {
				return ArgumentDescription.IsValid(Value);
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

	}

}
