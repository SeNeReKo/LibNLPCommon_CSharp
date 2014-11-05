using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io.tcf
{

	public class TCFPoSTag
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private string id;

		public TCFToken Token;
		public string PoS;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TCFPoSTag(string id, TCFToken token, string pos)
		{
			__SetID(id);
			this.Token = token;
			this.PoS = pos;
		}

		/**
		 * Constructor.
		 */
		public TCFPoSTag(TCFToken token, string pos)
		{
			this.Token = token;
			this.PoS = pos;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string ID
		{
			get {
				return id;
			}
			set {
				__SetID(value);
			}
		}

		public TCFToken Tokens
		{
			get {
				return this.Token;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __SetID(string id)
		{
			if ((id == null) || (id.Length == 0) || (id.Trim().Length != id.Length))
				throw new Exception("Invalid ID specified!");
			this.id = id;
		}

	}

}
