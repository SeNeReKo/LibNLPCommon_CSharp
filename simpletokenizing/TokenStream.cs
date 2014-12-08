using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LibNLPCSharp.simpletokenizing
{

	public class TokenStream
	{

		public struct TSeq
		{
			public readonly string Name;
			public readonly TokenPattern[] Sequence;
			public readonly int[] PatternContent;

			public TSeq(string name, params TokenPattern[] sequence)
			{
				this.Name = name;
				this.Sequence = sequence;

				List<int> ret = new List<int>();
				for (int i = 0; i < sequence.Length; i++) {
					if (sequence[i].IsContent)
						ret.Add(i);
				}
				PatternContent = ret.ToArray();
			}

		}

		public struct TSeqMatchResult
		{
			public static readonly TSeqMatchResult None = new TSeqMatchResult();

			public readonly bool IsMatch;
			public readonly string Name;
			public readonly Token[] Tokens;
			public readonly Token[] TokensContent;

			public TSeqMatchResult(string name, Token[] tokens, int[] indicesContent)
			{
				this.IsMatch = true;

				this.Name = name;
				this.Tokens = tokens;

				List<Token> ret = new List<Token>();
				foreach (int i in indicesContent) {
					ret.Add(tokens[i]);
				}
				TokensContent = ret.ToArray();
			}

		}

		public interface IMarker
		{
			void Reset();
		}

		private class Marker : IMarker
		{
			public readonly int Position;
			public readonly TokenStream TokenStream;

			public Marker(TokenStream tokenStream, int pos)
			{
				this.TokenStream = tokenStream;
				this.Position = pos;
			}

			public void Reset()
			{
				TokenStream.__ResetToPosition(Position);
			}
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		Token[] tokens;
		int pos;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public TokenStream(params Token[] tokens)
		{
			if ((tokens.Length == 0) || (tokens[tokens.Length - 1].GeneralType != EnumGeneralTokenType.EOS)) {
				this.tokens = new Token[tokens.Length + 1];
				tokens.CopyTo(this.tokens, 0);
				this.tokens[tokens.Length] = new Token(-1, "", EnumGeneralTokenType.EOS);
			} else {
				this.tokens = tokens;
			}
		}

		public TokenStream(Token[] _tokens, int tpos, int tlen)
		{
			// TODO: optimize this

			Token[] tokens = new Token[tlen];
			for (int i = 0; i < tlen; i++) {
				tokens[i] = _tokens[pos + i];
			}

			if ((tokens.Length == 0) || (tokens[tokens.Length - 1].GeneralType != EnumGeneralTokenType.EOS)) {
				this.tokens = new Token[tokens.Length + 1];
				tokens.CopyTo(this.tokens, 0);
				this.tokens[tokens.Length] = new Token(-1, "", EnumGeneralTokenType.EOS);
			} else {
				this.tokens = tokens;
			}
		}

		public TokenStream(IEnumerable<Token> tokens)
			: this(tokens.ToArray())
		{
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public int LengthInTokens
		{
			get {
				return tokens.Length - 1;
			}
		}

		public int TokensRemaining
		{
			get {
				return tokens.Length - pos - 1;
			}
		}

		public int TokenPosition
		{
			get {
				return pos;
			}
		}

		public int CharacterPosition
		{
			get {
				return tokens[pos].CharacterPosition;
			}
		}

		public int LineNumber
		{
			get {
				return tokens[pos].LineNumber;
			}
		}

		public bool HasMoreTokens
		{
			get {
				return pos != tokens.Length - 1;
			}
		}

		public bool IsEOS
		{
			get {
				return pos == tokens.Length - 1;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public void Skip(int count)
		{
			pos += count;
			if (pos > tokens.Length - 1) pos = tokens.Length - 1;
		}

		public IMarker Mark()
		{
			return new Marker(this, pos);
		}

		public Token Read()
		{
			if (pos >= tokens.Length - 1) return null;
			return tokens[pos++];
		}

		public Token Peek()
		{
			return tokens[pos];
		}

		public Token[] Preview(int min, int max)
		{
			int len = (tokens.Length - pos - 1);
			if (len < min) return null;
			if (len > max) len = max;

			Token[] ret = new Token[len];
			for (int i = 0; i < len; i++) {
				ret[i] = tokens[pos + i];
			}

			return ret;
		}

		public Token[] TryEatSequence(params TokenPattern[] sequence)
		{
			int len = (tokens.Length - pos - 1);
			if (len < sequence.Length) return null;
			len = sequence.Length;

			for (int i = 0; i < len; i++) {
				if (!sequence[i].Match(tokens[pos + i])) return null;
			}

			Token[] ret = new Token[len];
			for (int i = 0; i < len; i++) {
				ret[i] = tokens[pos + i];
			}

			pos += sequence.Length;

			return ret;
		}

		/// <summary>
		/// Try to eat a single token.
		/// </summary>
		/// <param name="patternText"></param>
		/// <param name="bAllowCaching">If <code>true</code> the token pattern once parsed will be returned from
		/// a cache. This implies that you do not modify that token pattern after it has been returned by this method.
		/// If <code>false</code> is specified parsing occurs every time and always a new token pattern object is
		/// returned.</param>
		/// <returns></returns>
		public Token TryEat(string patternText, bool bAllowCaching, bool bCaseSensitive)
		{
			return TryEat(TokenPatternBuilder.ParseSingle(patternText, bAllowCaching, bCaseSensitive));
		}

		public Token TryEat(TokenPattern p)
		{
			int len = (tokens.Length - pos - 1);
			if (len == 0) return null;

			if (!p.Match(tokens[pos])) return null;
			Token t = tokens[pos];
			pos++;

			return t;
		}

		public TokenStream StripSpaces()
		{
			List<Token> tokens = new List<Token>();
			foreach (Token t in this.tokens) {
				if (t.IsSpace) continue;
				tokens.Add(t);
			}
			return new TokenStream(tokens.ToArray());
		}

		public IEnumerable<TokenStream> SplitAtSpaces()
		{
			List<Token> tokens = new List<Token>();
			foreach (Token t in this.tokens) {
				if (t.IsSpace) {
					if (!(tokens.Count == 0) && !((tokens.Count == 1) && tokens[0].IsEOS)) {
						yield return new TokenStream(tokens.ToArray());
					}
					tokens.Clear();
				} else {
					tokens.Add(t);
				}
			}
			if (!(tokens.Count == 0) && !((tokens.Count == 1) && tokens[0].IsEOS)) {
				yield return new TokenStream(tokens.ToArray());
			}
		}

		public TokenStream DeriveSubStream(int tokenPosition)
		{
			List<Token> selected = new List<Token>();
			for (int i = pos + tokenPosition; i < tokens.Length; i++) {
				selected.Add(tokens[i]);
			}
			return new TokenStream(selected);
		}

		/// <summary>
		/// Return the remaining tokens (without advancing)
		/// </summary>
		/// <returns></returns>
		public Token[] GetRemainingTokensAsArray()
		{
			Token[] ret = new Token[tokens.Length - pos];
			for (int i = 0; i < ret.Length; i++) {
				ret[i] = tokens[i + pos];
			}
			return ret;
		}

		protected void __ResetToPosition(int pos)
		{
			this.pos = pos;
		}

		public TSeqMatchResult TryEatAlternatives(params TSeq[] sequences)
		{
			foreach (TSeq sequence in sequences) {
				Token[] tokens = TryEatSequence(sequence.Sequence);
				if (tokens != null) {
					return new TSeqMatchResult(sequence.Name, tokens, sequence.PatternContent);
				}
			}
			return TSeqMatchResult.None;
		}

	}

}
