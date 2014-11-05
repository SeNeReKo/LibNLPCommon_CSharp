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

		private static readonly LinearPatternSequence __WORDPATTERN1 = new LinearPatternSequence(
			TokenPattern.MatchWord("W", false),
			TokenPattern.MatchAnyStringSQ(true)
			);

		private static readonly LinearPatternSequence __WORDPATTERN2 = new LinearPatternSequence(
			TokenPattern.MatchWord("W", false),
			TokenPattern.MatchAnyStringDQ(true)
			);

		private static readonly LinearPatternSequence __WORDPATTERNANY = new LinearPatternSequence(
			TokenPattern.MatchWord("W", false),
			TokenPattern.MatchDelimiter("*", false)
			);

		private static readonly LinearPatternSequence __WORDPATTERN3 = new LinearPatternSequence(
			TokenPattern.MatchAnyStringSQ(true)
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

		public static TokenPattern __TryEatWord(TokenStream tokenStream)
		{
			TokenPatternResult p;

			if ((p = __WORDPATTERNANY.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchAnyWord();
			}

			if ((p = __WORDPATTERN1.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchWord(p.ContentTokens[0].Text);
			}

			if ((p = __WORDPATTERN2.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchWord(p.ContentTokens[0].Text);
			}

			if ((p = __WORDPATTERN3.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchWord(p.ContentTokens[0].Text);
			}

			return null;
		}

	}

}
