using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.simpletokenizing
{

	public partial class TokenPatternBuilder
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		private static readonly LinearPatternSequence __STRINGPATTERNANY = new LinearPatternSequence(
			TokenPattern.MatchWord("T", false),
			TokenPattern.MatchDelimiter("*", false)
			);

		private static readonly LinearPatternSequence __STRINGPATTERN1 = new LinearPatternSequence(
			TokenPattern.MatchWord("T", false),
			TokenPattern.MatchAnyStringSQ(true)
			);

		private static readonly LinearPatternSequence __STRINGPATTERN2 = new LinearPatternSequence(
			TokenPattern.MatchWord("T", false),
			TokenPattern.MatchAnyStringDQ(true)
			);

		private static readonly LinearPatternSequence __STRINGPATTERN3 = new LinearPatternSequence(
			TokenPattern.MatchAnyStringDQ(true)
			);

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static TokenPattern __TryEatString(TokenStream tokenStream)
		{
			TokenPatternResult p;

			if ((p = __STRINGPATTERNANY.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchAnyStringDQ();
			}

			if ((p = __STRINGPATTERN1.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchStringDQ(p.ContentTokens[0].Text);
			}

			if ((p = __STRINGPATTERN2.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchStringDQ(p.ContentTokens[0].Text);
			}

			if ((p = __STRINGPATTERN3.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchStringDQ(p.ContentTokens[0].Text);
			}

			return null;
		}

	}

}
