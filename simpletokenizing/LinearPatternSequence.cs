using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.simpletokenizing
{

	public class LinearPatternSequence : ITokenPatternMatcher
	{

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		TokenPattern[] sequence;
		int countContent;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		public LinearPatternSequence(params TokenPattern[] sequence)
			: this(null, sequence)
		{
		}

		public LinearPatternSequence(string name, params TokenPattern[] sequence)
		{
			this.Name = name;
			this.sequence = sequence;
			for (int i = 0; i < sequence.Length; i++) {
				if (sequence[i].IsContent)
					countContent++;
			}
		}

		public LinearPatternSequence(IEnumerable<TokenPattern> sequence)
			: this(null, sequence)
		{
		}

		public LinearPatternSequence(string name, IEnumerable<TokenPattern> sequence)
		{
			List<TokenPattern> seq = new List<TokenPattern>(sequence);
			this.sequence = seq.ToArray();
			for (int i = 0; i < this.sequence.Length; i++) {
				if (this.sequence[i].IsContent) countContent++;
			}
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Returns the fixed length of the sequence matched (if this pattern matcher has a fixed length)
		/// </summary>
		public int? Length
		{
			get {
				return sequence.Length;
			}
		}

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		public bool Match(TokenStream tokenStream)
		{
			Token[] tokens = tokenStream.Preview(sequence.Length, sequence.Length);
			if (tokens == null) return false;

			for (int i = 0; i < tokens.Length; i++) {
				if (!sequence[i].Match(tokens[i])) return false;
			}
			return true;
		}

		public bool Match(Token[] tokenList, int pos)
		{
			if (pos + sequence.Length > tokenList.Length) return false;

			for (int i = 0; i < sequence.Length; i++) {
				if (!sequence[i].Match(tokenList[pos + i])) return false;
			}
			return true;
		}

		/// <summary>
		/// Try to eat the stored pattern from the stream. On success the stream is advanced.
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		public TokenPatternResult TryEat(TokenStream tokenStream)
		{
			Token[] tokens = tokenStream.Preview(sequence.Length, sequence.Length);
			if (tokens == null) return null;

			for (int i = 0; i < tokens.Length; i++) {
				if (!sequence[i].Match(tokens[i])) return null;
			}

			Token[] ret = new Token[countContent];
			int j = 0;
			for (int i = 0; i < tokens.Length; i++) {
				TokenPattern tp = sequence[i];
				if (tp.IsContent) {
					Token token = tokens[i].CloneObject();
					ret[j] = token;
					if (tp.Tags != null) {
						token.Tags = new SKVPSet(false);
						token.Tags.AddRange(tp.Tags);
					}
					j++;
				}
			}

			tokenStream.Skip(tokens.Length);

			return new TokenPatternResult(Name, sequence, tokens, ret);
		}

		/*
		/// <summary>
		/// Same as <code>TryEat()</code> but without advancing the stream
		/// </summary>
		/// <param name="tokenStream"></param>
		/// <returns></returns>
		public TokenPatternResult TryMatch(TokenStream tokenStream)
		{
			Token[] tokens = tokenStream.Preview(sequence.Length, sequence.Length);
			if (tokens == null) return null;

			for (int i = 0; i < tokens.Length; i++) {
				if (!sequence[i].Match(tokens[i])) return null;
			}

			Token[] ret = new Token[countContent];
			int j = 0;
			for (int i = 0; i < tokens.Length; i++) {
				if (sequence[i].IsContent) ret[j++] = tokens[i];
			}

			return new TokenPatternResult(Name, tokens, ret);
		}
		*/

	}

}
