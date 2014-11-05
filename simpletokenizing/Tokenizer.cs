using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using LibNLPCSharp.util;


namespace LibNLPCSharp.simpletokenizing
{

	public class Tokenizer
	{

		public enum EnumSpaceProcessing
		{
			OriginalSpaces,
			RemoveDuplicateSpaces,
			SkipAllSpaces,
		}

		////////////////////////////////////////////////////////////////
		// Constants
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Variables
		////////////////////////////////////////////////////////////////

		bool bLowerizeCharacters;
		EnumSpaceProcessing spaceProcessing;
		string extraAlphabetLetters;
		bool bParseStrings;

		////////////////////////////////////////////////////////////////
		// Constructors
		////////////////////////////////////////////////////////////////

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="bLowerizeCharacters">Convert all characters to lower case using the standard <code>char.ToLower()</code> method</param>
		/// <param name="bRemoveDuplicateSpaces">Convert duplicate spaces to a single space character</param>
		/// <param name="bParseStrings">Recognize strings in the input data</param>
		/// <param name="extraAlphabetLetters">Extra alphabet letters to recognize more characters as letters
		/// than the default ones</param>
		public Tokenizer(bool bLowerizeCharacters, EnumSpaceProcessing spaceProcessing, bool bParseStrings,
			string extraAlphabetLetters)
		{
			this.bParseStrings = bParseStrings;
			this.spaceProcessing = spaceProcessing;
			this.bLowerizeCharacters = bLowerizeCharacters;
			this.extraAlphabetLetters = (extraAlphabetLetters == null) ? "" : extraAlphabetLetters;
		}

		////////////////////////////////////////////////////////////////
		// Properties
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		// Methods
		////////////////////////////////////////////////////////////////

		private void __CreateTokenFromBufferDontSkipIfEmpty(EnumGeneralTokenType tokenType, int lineNo, int posLastEmit, StringBuilder sb,
			List<Token> outTokenCollection)
		{
			switch (tokenType) {
				case EnumGeneralTokenType.StringSQ:
					outTokenCollection.Add(Token.CreateStringSQToken(lineNo, posLastEmit, sb.ToString()));
					break;
				case EnumGeneralTokenType.StringDQ:
					outTokenCollection.Add(Token.CreateStringDQToken(lineNo, posLastEmit, sb.ToString()));
					break;
				case EnumGeneralTokenType.Word:
					outTokenCollection.Add(Token.CreateWordToken(lineNo, posLastEmit, sb.ToString()));
					break;
				default:
					throw new Exception();
			}
			sb.Remove(0, sb.Length);
		}

		private void __CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType tokenType, int lineNo, int posLastEmit, StringBuilder sb,
			List<Token> outTokenCollection)
		{
			if (sb.Length == 0) return;

			switch (tokenType) {
				case EnumGeneralTokenType.StringSQ:
					outTokenCollection.Add(Token.CreateStringSQToken(lineNo, posLastEmit, sb.ToString()));
					break;
				case EnumGeneralTokenType.StringDQ:
					outTokenCollection.Add(Token.CreateStringDQToken(lineNo, posLastEmit, sb.ToString()));
					break;
				case EnumGeneralTokenType.Word:
					outTokenCollection.Add(Token.CreateWordToken(lineNo, posLastEmit, sb.ToString()));
					break;
				default:
					throw new Exception();
			}
			sb.Remove(0, sb.Length);
		}

		public Token[] Tokenize(string input)
		{
			return Tokenize(-1, input);
		}

		public Token[] Tokenize(int lineNo, string input)
		{
			/*
			if (input.StartsWith("[a + paṭi°]")) {
				int xx = 0;
			}
			*/

			List<Token> tokens = new List<Token>();
			StringBuilder sb = new StringBuilder();

			bool bInSingleQuotes = false;
			bool bInDoubleQuotes = false;
			bool bMasked = false;

			int pos = 0;
			int posLastBegin = 1;
			foreach (char c2 in input) {
				pos++;
				char c = bLowerizeCharacters ? char.ToLower(c2) : c2;

				// ----

				if (bMasked) {
					sb.Append(c);
					bMasked = false;
					continue;
				}

				if (bInSingleQuotes) {
					if (c == '\'') {
						__CreateTokenFromBufferDontSkipIfEmpty(EnumGeneralTokenType.StringSQ, lineNo, posLastBegin, sb, tokens);
						bInSingleQuotes = false;
					} else
					if (c == '\\') {
						if (sb.Length == 0) posLastBegin = pos;
						bMasked = true;
					} else {
						if (sb.Length == 0) posLastBegin = pos;
						sb.Append(c);
					}
					continue;
				}

				if (bInDoubleQuotes) {
					if (c == '\"') {
						__CreateTokenFromBufferDontSkipIfEmpty(EnumGeneralTokenType.StringDQ, lineNo, posLastBegin, sb, tokens);
						bInDoubleQuotes = false;
					} else
					if (c == '\\') {
						if (sb.Length == 0) posLastBegin = pos;
						bMasked = true;
					} else {
						if (sb.Length == 0) posLastBegin = pos;
						sb.Append(c);
					}
					continue;
				}

				// ----

				if (bParseStrings) {
					if (c == '\'') {
						__CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType.Word, lineNo, posLastBegin, sb, tokens);
						bInSingleQuotes = true;
						continue;
					} else
					if (c == '\"') {
						__CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType.Word, lineNo, posLastBegin, sb, tokens);
						bInDoubleQuotes = true;
						continue;
					}
				}

				// ----
				
				if (char.IsWhiteSpace(c) || (c == 13) || (c == 10)) {
					__CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType.Word, lineNo, posLastBegin, sb, tokens);

					if (spaceProcessing == EnumSpaceProcessing.OriginalSpaces) {
						tokens.Add(Token.CreateSpaceToken(lineNo, pos));
					} else
					if (spaceProcessing == EnumSpaceProcessing.RemoveDuplicateSpaces) {
						if ((tokens.Count > 0) && tokens[tokens.Count - 1].IsSpace) {
							continue;
						}
						tokens.Add(Token.CreateSpaceToken(lineNo, pos));
					} else
					if (spaceProcessing == EnumSpaceProcessing.SkipAllSpaces) {
					} else {
						throw new ImplementationErrorException();
					}

					if (c == 13) {
						lineNo++;
					}

					continue;
				}

				// ----

				if (char.IsLetterOrDigit(c) || (extraAlphabetLetters.IndexOf(char.ToLower(c)) >= 0)) {
					if (sb.Length == 0) posLastBegin = pos;
					sb.Append(c);
					continue;
				}

				// ----

				__CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType.Word, lineNo, posLastBegin, sb, tokens);

				tokens.Add(Token.CreateDelimiterToken(lineNo, pos, c));
			}

			if (bMasked) throw new Exception("Unclosed string literal at " + lineNo + ":" + pos);
			if (bInSingleQuotes) throw new Exception("Unclosed string literal at: " + lineNo + ":" + pos);
			if (bInDoubleQuotes) throw new Exception("Unclosed string literal at: " + lineNo + ":" + pos);

			__CreateTokenFromBufferSkipIfEmpty(EnumGeneralTokenType.Word, lineNo, posLastBegin, sb, tokens);

			tokens.Add(new Token(lineNo, pos, "", EnumGeneralTokenType.EOS));

			return tokens.ToArray();
		}

	}

}
