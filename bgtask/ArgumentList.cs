using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.bgtask
{

	public class ArgumentList : List<Argument>
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

		public ArgumentList(params Argument[] arguments)
			: base(arguments)
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public Argument this[string id]
		{
			get {
				for (int i = 0; i < Count; i++) {
					if (this[i].ID.Equals(id)) return this[i];
				}
				throw new Exception("No such argument: " + id);
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public string GetValueE(string id)
		{
			for (int i = 0; i < Count; i++) {
				if (this[i].ID.Equals(id)) {
					string s = this[i].Value;
					if (s == null) break;
					return s;
				}
			}
			throw new Exception("No such argument: " + id);
		}

		public string GetValue(string id)
		{
			for (int i = 0; i < Count; i++) {
				if (this[i].ID.Equals(id)) {
					return this[i].Value;
				}
			}
			return null;
		}

	}

}
