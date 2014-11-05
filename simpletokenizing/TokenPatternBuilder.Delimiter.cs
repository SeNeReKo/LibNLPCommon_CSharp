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

		private static readonly LinearPatternSequence __DELIMITERPATTERNANY = new LinearPatternSequence(
			TokenPattern.MatchWord("D", false),
			TokenPattern.MatchDelimiter("*", false)
			);

		private static readonly LinearPatternSequence __DELIMITERPATTERN1 = new LinearPatternSequence(
			TokenPattern.MatchWord("D", false),
			TokenPattern.MatchAnyStringSQ(true)
			);

		private static readonly LinearPatternSequence __DELIMITERPATTERN2 = new LinearPatternSequence(
			TokenPattern.MatchWord("D", false),
			TokenPattern.MatchAnyStringDQ(true)
			);

		private static readonly LinearPatternSequence __DELIMITERPATTERN3 = new LinearPatternSequence(
			TokenPattern.MatchAnyDelimiter(true)
			);

		private static readonly LinearPatternSequence __DELIMITERPATTERN4 = new LinearPatternSequence(
			TokenPattern.MatchWord("D", false),
			TokenPattern.MatchAnyDelimiter(true)
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

		public static TokenPattern __TryEatDelimiter(TokenStream tokenStream, string text)
		{
			TokenPatternResult p;

			if ((p = __DELIMITERPATTERNANY.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchAnyDelimiter();
			}

			if ((p = __DELIMITERPATTERN1.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchDelimiter(__ToDelimChar(p.ContentTokens[0], text));
			}

			if ((p = __DELIMITERPATTERN2.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchDelimiter(p.ContentTokens[0].Text);
			}

			if ((p = __DELIMITERPATTERN3.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchDelimiter(__ToDelimChar(p.ContentTokens[0], text));
			}

			if ((p = __DELIMITERPATTERN4.TryEat(tokenStream)) != null) {
				return TokenPattern.MatchDelimiter(__ToDelimChar(p.ContentTokens[0], text));
			}

			return null;
		}

	}

}
