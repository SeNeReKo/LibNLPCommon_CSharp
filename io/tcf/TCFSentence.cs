using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.io.tcf
{

	public class TCFSentence
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private string id;

		private List<TCFToken> tokens;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TCFSentence(string id, List<TCFToken> tokens)
		{
			__SetID(id);
			this.tokens = new List<TCFToken>();
			if (tokens != null) this.tokens.AddRange(tokens);
		}

		public TCFSentence(string id, params TCFToken[] tokens)
		{
			__SetID(id);
			this.tokens = new List<TCFToken>(tokens);
		}

		public TCFSentence(List<TCFToken> tokens)
		{
			this.tokens = new List<TCFToken>();
			if (tokens != null) this.tokens.AddRange(tokens);
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

		public TCFToken[] Tokens
		{
			get {
				return tokens.ToArray();
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
