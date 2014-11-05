using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.simpletokenizing
{

	public interface ITokenPatternMatcher
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

		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the fixed length of the sequence matched (if this pattern matcher has a fixed length)
		/// </summary>
		int? Length
		{
			get;
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		bool Match(TokenStream tokenStream);

		/// <summary>
		/// Try to eat the stored pattern from the stream. On success the stream is advanced.
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		TokenPatternResult TryEat(TokenStream tokenStream);

		/*
		/// <summary>
		/// Same as <code>TryEat()</code> but without advancing the stream
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		TokenPatternResult TryMatch(TokenStream tokenStream);
		*/

	}

}
