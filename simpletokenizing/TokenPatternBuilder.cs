using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


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

		private static Tokenizer tokenizer = new Tokenizer(false, Tokenizer.EnumSpaceProcessing.RemoveDuplicateSpaces, true, "");

		private static readonly TokenPattern PATTERN_EXCLAMATIONMARK = TokenPattern.MatchDelimiter('!');
		private static readonly LinearPatternSequence PATTERN_KVP = new LinearPatternSequence(
			TokenPattern.MatchAnyWord(true),
			TokenPattern.MatchDelimiter(':', false),
			TokenPattern.MatchAnyWord(true)
			);

		private static Dictionary<string, TokenPattern> singleTokenCache = new Dictionary<string, TokenPattern>();

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		private TokenPatternBuilder()
		{
		}
		
		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public static LinearPatternSequenceCollection Parse(params string[] text)
		{
			return Parse(true, text);
		}

		public static LinearPatternSequenceCollection Parse(bool bCaseSensitive, params string[] text)
		{
			LinearPatternSequenceCollection c = new LinearPatternSequenceCollection();
			foreach (string s in text) {
				LinearPatternSequence lpc = Parse(bCaseSensitive, s);
				if (lpc != null) c.Add(lpc);
			}
			c.Sort();
			return c;
		}

		public static LinearPatternSequenceCollection Parse(IEnumerable<string> text)
		{
			return Parse(true, text);
		}

		public static LinearPatternSequenceCollection Parse(bool bCaseSensitive, IEnumerable<string> text)
		{
			LinearPatternSequenceCollection c = new LinearPatternSequenceCollection();
			foreach (string s in text) {
				LinearPatternSequence lpc = Parse(bCaseSensitive, s);
				if (lpc != null) c.Add(lpc);
			}
			c.Sort();
			return c;
		}

		/// <summary>
		/// Parse a text that contains information about a single token.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="bAllowCaching">If <code>true</code> the token pattern once parsed will be returned from
		/// a cache. This implies that you do not modify that token pattern after it has been returned by this method.
		/// If <code>false</code> is specified parsing occurs every time and always a new token pattern object is
		/// returned.</param>
		/// <returns></returns>
		public static TokenPattern ParseSingle(string text, bool bAllowCaching, bool bCaseSensitive)
		{
			if (bAllowCaching) {
				TokenPattern tp;
				if (singleTokenCache.TryGetValue(text, out tp)) return tp;
				tp = __ParseSingle(text, bCaseSensitive);
				singleTokenCache.Add(text, tp);
				return tp;
			} else {
				return __ParseSingle(text, bCaseSensitive);
			}
		}

		private static TokenPattern __ParseSingle(string text, bool bCaseSensitive)
		{
			TokenStream ts = new TokenStream(tokenizer.Tokenize(text));
			while (ts.Peek().IsSpace) ts.Read();

			TokenPattern tp = EatTokenPattern(ts, text, bCaseSensitive);

			if (!ts.IsEOS) {
				// try to parse content relevancy information

				if (ts.TryEat(PATTERN_EXCLAMATIONMARK) != null) {
					tp.IsContent = true;
				} else {
					throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");
				}
			}

			if (!ts.IsEOS) {
				// try to parse key value pair

				TokenPatternResult extra;
				if ((extra = PATTERN_KVP.TryEat(ts)) != null) {
					tp.Tags = new SKVPSet(true, new SKVP(extra.ContentTokens[0].Text, extra.ContentTokens[1].Text));
				} else {
					throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");
				}
			}

			while (ts.Peek().IsSpace) ts.Read();

			if (!ts.IsEOS)
				throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");

			return tp;
		}

		public static LinearPatternSequence Parse(string text)
		{
			return Parse(true, text);
		}

		public static LinearPatternSequence Parse(bool bCaseSensitive, string text)
		{
			TokenStream tokenStream = new TokenStream(tokenizer.Tokenize(text));

			List<TokenPattern> parsedPatterns = new List<TokenPattern>();

			foreach (TokenStream ts in tokenStream.SplitAtSpaces()) {

				TokenPattern tp = EatTokenPattern(ts, text, bCaseSensitive);

				if (!ts.IsEOS) {
					// try to parse content relevancy information

					if (ts.TryEat(PATTERN_EXCLAMATIONMARK) != null) {
						tp.IsContent = true;
					} else {
						throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");
					}
				}

				if (!ts.IsEOS) {
					// try to parse key value pair

					TokenPatternResult extra;
					if ((extra = PATTERN_KVP.TryEat(ts)) != null) {
						tp.Tags = new SKVPSet(true, new SKVP(extra.ContentTokens[0].Text, extra.ContentTokens[1].Text));
					} else {
						throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");
					}
				}

				if (!ts.IsEOS)
					throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");

				parsedPatterns.Add(tp);

			}

			if (parsedPatterns.Count == 0) return null;
			return new LinearPatternSequence(parsedPatterns);
		}

		private static TokenPattern EatTokenPattern(TokenStream ts, string text, bool bCaseSensitive)
		{
			TokenPattern tp = __EatTokenPattern0(ts, text);
			if (!bCaseSensitive) tp = tp.ToCaseInsensitive();
			return tp;
		}

		private static TokenPattern __EatTokenPattern0(TokenStream ts, string text)
		{
			TokenPattern tp;
			if ((tp = __TryEatSpace(ts)) != null) return tp;
			if ((tp = __TryEatString(ts)) != null) return tp;
			if ((tp = __TryEatWord(ts)) != null) return tp;
			if ((tp = __TryEatDelimiter(ts, text)) != null) return tp;

			throw new Exception("Syntax error at character position: " + ts.CharacterPosition + " (" + ts.Peek().ToString() + ") [\"" + text + "\"]");
		}

		private static bool __ToIsContent(Token token, string text)
		{
			if (token.Text.Equals(".")) return false;
			if (token.Text.Equals("!")) return true;
			throw new Exception("Syntax error at character position: " + token.CharacterPosition + " (" + token.ToString() + ") [\"" + text + "\"]");
		}

		private static char __ToDelimChar(Token token, string text)
		{
			if (token.Text.Length != 1) {
				throw new Exception("Syntax error at character position: " + token.CharacterPosition + " (" + token.ToString() + ") [\"" + text + "\"]");
			}
			return token.Text[0];
		}

	}

}
